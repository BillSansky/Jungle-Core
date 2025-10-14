﻿#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    public sealed class TypeSelectableField : BindableElement
    {
        private bool allowRowToggle;

        
        // --- UI ---
        private readonly Toggle expandToggle; 
        private readonly Label label; 

        private readonly VisualElement
            content; 

        private readonly Button btnPickOrSwap; // "+" (pick) or "↺" (swap)
        private readonly Button btnClear; // "✕"

        // Under-row details host (the "folded" section)
        private readonly VisualElement underRowHost;

        // --- Binding state ---
        private SerializedProperty prop;
        private Type baseType;
        private bool isManagedRef;

        private EventCallback<ChangeEvent<bool>> expandToggleHandler;

        // Track reference state to detect structural changes
        private object lastManagedRefValue;
        private Type lastManagedRefType;
        private UnityEngine.Object lastObjectRefValue;

        // Guard in case some drawer mistakenly applies to children
        [ThreadStatic] private static bool renderingChildren;

        public TypeSelectableField()
        {
            // Load Unity-style stylesheet
            // The stylesheet should be placed in a Resources folder, e.g., "Assets/Plugins/Jungle Core/Editor/Resources/TypeSelectableFieldStyle.uss"
            var styleSheet = Resources.Load<StyleSheet>("TypeSelectableFieldStyle");
            if (styleSheet != null)
                styleSheets.Add(styleSheet);

            AddToClassList("tsf-root");

            var rootContainer = new VisualElement();
            Add(rootContainer);

            var row = new VisualElement();
            row.AddToClassList("tsf-row");
            row.name = "tsf-row";

            // Foldout toggle (arrow)
            expandToggle = new Toggle { value = false, text = "", focusable = false, tooltip = "Show details" };
            expandToggle.AddToClassList("tsf__toggle");
            expandToggle.AddToClassList("unity-foldout__toggle");
            row.Add(expandToggle);

            // Property label
            label = new Label();
            label.AddToClassList("tsf__label");
            label.AddToClassList("unity-property-field__label");
            row.Add(label);

            // Content area (ObjectField or type summary)
            content = new VisualElement { name = "tsf-content" };
            content.AddToClassList("tsf__content");
            row.Add(content);

            // Button group (pick/swap and clear)
            var buttonGroup = new VisualElement();
            buttonGroup.AddToClassList("tsf__button-group");
            row.Add(buttonGroup);

            btnPickOrSwap = new Button(OnPickOrSwapClicked) { text = "+" };
            btnPickOrSwap.tooltip = "Pick or change the type";
            btnPickOrSwap.AddToClassList("tsf__button");
            btnPickOrSwap.RegisterCallback<ClickEvent>(evt => evt.StopPropagation());
            buttonGroup.Add(btnPickOrSwap);

            btnClear = new Button(OnClearClicked) { text = "✕" };
            btnClear.tooltip = "Clear";
            btnClear.AddToClassList("tsf__button");
            btnClear.RegisterCallback<ClickEvent>(evt => evt.StopPropagation());
            buttonGroup.Add(btnClear);

            rootContainer.Add(row);

            // Foldout details container
            underRowHost = new VisualElement();
            underRowHost.AddToClassList("tsf__details");
            underRowHost.name = "tsf-details";
            rootContainer.Add(underRowHost);

            // Click handler for label to toggle foldout
            row.RegisterCallback<ClickEvent>(evt =>
            {
                if (!allowRowToggle) return;
                expandToggle.value = !expandToggle.value;
           
                evt.StopPropagation();
            });

            
        }

        /// <summary>Bind the field to a property and base type.</summary>
        public void Bind(SerializedProperty property, Type baseType)
        {
            prop = property ?? throw new ArgumentNullException(nameof(property));

            this.baseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
            isManagedRef = prop.propertyType == SerializedPropertyType.ManagedReference;
            allowRowToggle = false; 
            
           // bindingPath = prop.propertyPath;

            label.text = string.IsNullOrEmpty(prop.displayName) ? prop.name : prop.displayName;
            label.tooltip = prop.tooltip;

            // Persisted expand state per property
            string key = $"TSF_Expanded::{prop.serializedObject.targetObject.GetInstanceID()}::{prop.propertyPath}";
            bool expanded = EditorPrefs.GetBool(key, false);
            expandToggle.SetValueWithoutNotify(expanded);
            underRowHost.style.display = expanded ? DisplayStyle.Flex : DisplayStyle.None;

            // Avoid duplicate handlers on rebind
            if (expandToggleHandler != null)
                expandToggle.UnregisterValueChangedCallback(expandToggleHandler);

            expandToggleHandler = evt =>
            {
                EditorPrefs.SetBool(key, evt.newValue);
                underRowHost.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;
            };
            expandToggle.RegisterValueChangedCallback(expandToggleHandler);

            // First paint
            RepaintContentOnly();
            RefreshButtons();

            // Capture initial reference state
            CaptureReferenceState();

            // Keep in sync with changes - only repaint on structural changes
            this.TrackPropertyValue(prop, _ =>
            {
                if (HasStructuralChange())
                {
                    RepaintContentOnly();
                    RefreshButtons();
                    CaptureReferenceState();
                }
            });

            this.TrackSerializedObjectValue(prop.serializedObject, _ =>
            {
                if (HasStructuralChange())
                {
                    RepaintContentOnly();
                    RefreshButtons();
                    CaptureReferenceState();
                }
            });
        }

        // ---------------- Buttons ----------------


        private void OnPickOrSwapClicked()
        {
            if (prop == null) return;
            Debug.Log("CLICK");

            if (isManagedRef)
            {
                // Unchanged: managed-ref path uses your existing searchable picker
                TypePickerDropdown.Show(btnPickOrSwap.worldBound, baseType, t =>
                {
                    var so = prop.serializedObject;
                    so.Update();
                    if (t == null)
                    {
                        prop.managedReferenceValue = null;
                    }
                    else
                    {
                        try
                        {
                            prop.managedReferenceValue = Activator.CreateInstance(t);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }

                    so.ApplyModifiedProperties();
                    schedule.Execute(RepaintContentOnly);
                    schedule.Execute(RefreshButtons);
                });
                return;
            }

            // NEW: UnityEngine.Object path also opens the same type picker
            TypePickerDropdown.Show(btnPickOrSwap.worldBound, baseType, pickedType =>
            {
                if (pickedType == null) return;

                var so = prop.serializedObject;
                so.Update();

                UnityEngine.Object createdOrAssigned = null;

                try
                {
                    // ScriptableObject
                    if (typeof(ScriptableObject).IsAssignableFrom(pickedType))
                    {
                        var instance = ScriptableObject.CreateInstance(pickedType);

                        // Propose a sensible default folder (selected folder or Assets)
                        string defaultFolder = "Assets";
                        string selPath = AssetDatabase.GetAssetPath(Selection.activeObject);
                        if (!string.IsNullOrEmpty(selPath))
                        {
                            defaultFolder = Path.HasExtension(selPath)
                                ? Path.GetDirectoryName(selPath)
                                : selPath;
                        }

                        string path = EditorUtility.SaveFilePanelInProject(
                            $"Create {pickedType.Name}",
                            $"{pickedType.Name}.asset",
                            "asset",
                            "Choose a save location",
                            defaultFolder
                        );

                        if (!string.IsNullOrEmpty(path))
                        {
                            AssetDatabase.CreateAsset(instance, path);
                            AssetDatabase.SaveAssets();
                            EditorGUIUtility.PingObject(instance);
                            createdOrAssigned = instance;
                        }
                        else
                        {
                            // user cancelled: clean up the temp instance
                            UnityEngine.Object.DestroyImmediate(instance);
                        }
                    }
                    // Component / MonoBehaviour
                    else if (typeof(Component).IsAssignableFrom(pickedType) ||
                             typeof(MonoBehaviour).IsAssignableFrom(pickedType))
                    {
                        var go = ResolveContextGameObject(prop);
                        if (go == null)
                        {
                            EditorUtility.DisplayDialog(
                                "Select a GameObject",
                                "No suitable GameObject found. Select a GameObject in the Hierarchy and try again.",
                                "OK"
                            );
                        }
                        else
                        {
                            Undo.RecordObject(go, "Add Component");
                            var comp = Undo.AddComponent(go, pickedType) as Component;
                            createdOrAssigned = comp;
                            // Keep inspector focused on where the component was added
                            Selection.activeGameObject = go;
                        }
                    }
                    // Fallback: other UnityEngine.Object types (Textures, Materials, etc.) – not auto-created
                    else if (typeof(UnityEngine.Object).IsAssignableFrom(pickedType))
                    {
                        EditorUtility.DisplayDialog(
                            "Unsupported Instantiation",
                            $"Cannot automatically create an instance of {pickedType.Name}. Assign via the Object field instead.",
                            "OK"
                        );
                    }
                    else
                    {
                        EditorUtility.DisplayDialog(
                            "Invalid Type",
                            $"{pickedType.Name} is not a UnityEngine.Object.",
                            "OK"
                        );
                    }

                    if (createdOrAssigned != null)
                    {
                        prop.objectReferenceValue = createdOrAssigned;
                        so.ApplyModifiedProperties();
                        schedule.Execute(RepaintContentOnly);
                        schedule.Execute(RefreshButtons);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            });
        }

// Helpers (put inside the same class)
        private static GameObject ResolveContextGameObject(SerializedProperty p)
        {
            var target = p.serializedObject.targetObject;
            if (target is Component c) return c.gameObject;
            if (target is GameObject go) return go;
            return Selection.activeGameObject; // fallback
        }

        private void OnClearClicked()
        {
            if (prop == null) return;

            var so = prop.serializedObject;
            so.Update();

            if (isManagedRef)
                prop.managedReferenceValue = null;
            else
                prop.objectReferenceValue = null;

            so.ApplyModifiedProperties();

            RepaintContentOnly();
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            if (prop == null) return;

            bool hasValue = !prop.hasMultipleDifferentValues && (
                isManagedRef
                    ? prop.managedReferenceValue != null || !string.IsNullOrEmpty(prop.managedReferenceFullTypename)
                    : prop.objectReferenceValue != null);

            if (isManagedRef && hasValue)
            {
                btnPickOrSwap.text =  "↺";
                btnPickOrSwap.style.fontSize = 11;
            }
            else
            {
                btnPickOrSwap.text =  "+";
                btnPickOrSwap.style.fontSize = 14;
            }
            
          
            btnClear.style.display = hasValue ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private void CaptureReferenceState()
        {
            if (prop == null) return;

            if (isManagedRef)
            {
                lastManagedRefValue = prop.managedReferenceValue;
                lastManagedRefType = lastManagedRefValue?.GetType();
            }
            else
            {
                lastObjectRefValue = prop.objectReferenceValue;
            }
        }

        private bool HasStructuralChange()
        {
            if (prop == null) return false;

            if (isManagedRef)
            {
                var current = prop.managedReferenceValue;
                var currentType = current?.GetType();

                // Structural change if:
                // 1. null ↔ non-null transition
                // 2. Type changed
                bool wasNull = lastManagedRefValue == null;
                bool isNull = current == null;

                if (wasNull != isNull)
                    return true;

                if (lastManagedRefType != currentType)
                    return true;

                return false;
            }
            else
            {
                var current = prop.objectReferenceValue;

                // Structural change if reference changed (including null ↔ non-null)
                return lastObjectRefValue != current;
            }
        }

        // --------------- Content rendering ---------------

        private void RepaintContentOnly()
        {
            Debug.Log("REPAINT");
            content.Clear();
            underRowHost.Clear();

            if (prop == null) return;

            // Detect DrawInline from the managed reference type's JungleClassInfoAttribute
            bool drawInline = false;
            if (isManagedRef && prop.managedReferenceValue != null)
            {
                var valueType = prop.managedReferenceValue.GetType();
                var classInfo = (Jungle.Attributes.JungleClassInfoAttribute)Attribute.GetCustomAttribute(
                    valueType, typeof(Jungle.Attributes.JungleClassInfoAttribute));
                if (classInfo != null)
                {
                    drawInline = classInfo.DrawInline;
                }
            }

            // If values differ across multi-object selection: show mixed and hide foldout
            if (prop.hasMultipleDifferentValues)
            {
                var mixedLabel = new Label("—");
                mixedLabel.AddToClassList("tsf__mixed-value");
                content.Add(mixedLabel);
                expandToggle.style.display = DisplayStyle.None;
                underRowHost.style.display = DisplayStyle.None;
                label.style.display = drawInline ? DisplayStyle.None : DisplayStyle.Flex;
                return;
            }

            if (isManagedRef)
            {
                bool empty = prop.managedReferenceValue == null &&
                             string.IsNullOrEmpty(prop.managedReferenceFullTypename);

                if (empty)
                {
                    allowRowToggle = false;
                    expandToggle.style.display = DisplayStyle.None;
                    expandToggle.SetEnabled(false);
                    underRowHost.style.display = DisplayStyle.None;

                    label.style.display = drawInline ? DisplayStyle.None : DisplayStyle.Flex;

                    var none = new Label("None");
                    none.AddToClassList("tsf__empty-value");
                    content.Add(none);
                    return;
                }

                // Inline mode: hide base label, hide toggle, render children directly in content with parent label on first child
                if (drawInline)
                {
                    allowRowToggle = false;
                    expandToggle.style.display = DisplayStyle.None;
                    expandToggle.SetEnabled(false);
                    label.style.display = DisplayStyle.None;
                    underRowHost.style.display = DisplayStyle.None;

                    // Render children directly in content area, transferring parent label to first child
                    RenderManagedRefChildrenInto(prop, content, hideChildLabels: true, parentLabel: label.text);
                }
                else
                {
                    allowRowToggle = true;
                    expandToggle.style.display = DisplayStyle.Flex;
                    expandToggle.SetEnabled(true);
                    label.style.display = DisplayStyle.Flex;

                    // Row summary = nice type name
                    var typeNice = GetManagedRefNiceName(prop);
                    var summary = new Label(typeNice);
                    summary.AddToClassList("tsf__type-summary");
                    content.Add(summary);

                    // Details (children) go under the row
                    RenderManagedRefChildrenInto(prop, underRowHost, hideChildLabels: false);
                }
            }
            else
            {
                allowRowToggle = false;
                expandToggle.SetValueWithoutNotify(false);
                expandToggle.style.display = DisplayStyle.None;
                expandToggle.SetEnabled(false);
                label.style.display = DisplayStyle.Flex;

                var of = new ObjectField
                {
                    objectType = baseType,
                    allowSceneObjects = true
                };
                of.BindProperty(prop);
                content.Add(of);
            }
        }

        private static string GetManagedRefNiceName(SerializedProperty p)
        {
            // managedReferenceFullTypename is typically "AssemblyName Type.Full.Name"
            var full = p.managedReferenceFullTypename;
            if (string.IsNullOrEmpty(full)) return "None";
            int space = full.IndexOf(' ');
            var typeFull = space >= 0 ? full.Substring(space + 1) : full;
            int dot = typeFull.LastIndexOf('.');
            var shortName = dot >= 0 ? typeFull.Substring(dot + 1) : typeFull;
            return ObjectNames.NicifyVariableName(shortName);
        }

        private static void RenderManagedRefChildrenInto(SerializedProperty managedRefProp, VisualElement host, bool hideChildLabels = false, string parentLabel = null)
        {
            if (managedRefProp == null) return;
            if (renderingChildren) return;

            renderingChildren = true;
            try
            {
                var so = managedRefProp.serializedObject;
                so.Update();

                var it = managedRefProp.Copy();
                var end = it.GetEndProperty();
                int parentDepth = managedRefProp.depth;

                // Move to first child
                if (!it.NextVisible(true) || it.depth <= parentDepth)
                    return;

                bool isFirstChild = true;
                while (it.propertyPath != end.propertyPath && it.depth > parentDepth)
                {
                    // Draw children (not the parent)
                    string childLabel;
                    if (hideChildLabels && isFirstChild && !string.IsNullOrEmpty(parentLabel))
                    {
                        // First child gets the parent's label for drag-and-drop
                        childLabel = parentLabel;
                        isFirstChild = false;
                    }
                    else if (hideChildLabels)
                    {
                        // Subsequent children have no label
                        childLabel = "";
                    }
                    else
                    {
                        // Normal mode: use nicified child name
                        childLabel = ObjectNames.NicifyVariableName(it.name);
                    }

                    var child = new PropertyField(it.Copy(), childLabel);
                    child.AddToClassList("tsf__child-field");
                    if (hideChildLabels)
                    {
                        child.AddToClassList("tsf__child-field--inline");
                    }
                    child.Bind(so);
                    host.Add(child);

                    if (!it.NextVisible(false))
                        break;
                }
            }
            finally
            {
                renderingChildren = false;
            }
        }
    }
}
#endif