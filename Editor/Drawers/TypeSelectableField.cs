#if UNITY_EDITOR
using System;
using System.IO;
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

        // Guard in case some drawer mistakenly applies to children
        [ThreadStatic] private static bool renderingChildren;

        public TypeSelectableField()
        {
           // AddToClassList("jungle-editor");
            
            var input1 = new VisualElement();
            Add(input1);

            var row1 = new VisualElement();
            row1.AddToClassList("jungle-section");
            row1.AddToClassList("jungle-inline-wrapper");

            expandToggle = new Toggle { value = false, text = "", focusable = false, tooltip = "Show details" };
            expandToggle.AddToClassList("tsf__toggle");
            expandToggle.AddToClassList("unity-foldout__toggle");
            row1.Add(expandToggle); // first in row

            // Label next to the toggle
            label = new Label();
            label.AddToClassList("unity-base-field__label");
            row1.Add(label);

            // Right content host (summary)
            content = new VisualElement { name = "tsf-content" };
            content.AddToClassList("jungle-inline-content");
            row1.Add(content);

            // Buttons on the far right (keep your class)
            var btns1 = new VisualElement();
            btns1.AddToClassList("jungle-inline-button-group");
            row1.Add(btns1);

            btnPickOrSwap = new Button(OnPickOrSwapClicked) { text = "+" };
            btnPickOrSwap.tooltip = "Pick or change the type";
            btnPickOrSwap.AddToClassList("jungle-button");
            btns1.Add(btnPickOrSwap);

            btnClear = new Button(OnClearClicked) { text = "✕" };
            btnClear.tooltip = "Clear";
            btnClear.style.fontSize = 10;
            btnClear.AddToClassList("jungle-button");
            btns1.Add(btnClear);

            input1.Add(row1);
            
            underRowHost = new VisualElement();
            underRowHost.AddToClassList("jungle-foldout-group");
            input1.Add(underRowHost);


            row1.RegisterCallback<ClickEvent>(evt =>
            {
                if (!allowRowToggle) return; // <-- ignore clicks in object mode or when empty

                if (evt.target is VisualElement ve && (btns1.Contains(ve) || ve == btnPickOrSwap || ve == btnClear))
                    return;

                expandToggle.value = !expandToggle.value;
                evt.StopPropagation();
            });


            btnPickOrSwap.RegisterCallback<ClickEvent>(evt => evt.StopPropagation());
            btnClear.RegisterCallback<ClickEvent>(evt => evt.StopPropagation());
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

            // Keep in sync with changes
            this.TrackPropertyValue(prop, _ =>
            {
                RepaintContentOnly();
                RefreshButtons();
            });

            this.TrackSerializedObjectValue(prop.serializedObject, _ =>
            {
                RepaintContentOnly();
                RefreshButtons();
            });
        }

        // ---------------- Buttons ----------------


        private void OnPickOrSwapClicked()
        {
            if (prop == null) return;

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

        // --------------- Content rendering ---------------

        private void RepaintContentOnly()
        {
            content.Clear();
            underRowHost.Clear();

            if (prop == null) return;

            // If values differ across multi-object selection: show mixed and hide foldout
            if (prop.hasMultipleDifferentValues)
            {
                content.Add(new Label("—") { name = "mixed" });
                expandToggle.style.display = DisplayStyle.None;
                underRowHost.style.display = DisplayStyle.None;
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

                    var none = new Label("None");
                    none.AddToClassList("jungle-empty-value");
                    content.Add(none);
                    return;
                }

                allowRowToggle = true;
                expandToggle.style.display = DisplayStyle.Flex;
                expandToggle.SetEnabled(true);
                
                // Row summary = nice type name
                var typeNice = GetManagedRefNiceName(prop);
                var summary = new Label(typeNice);
                summary.AddToClassList("tsf__type-summary");
                content.Add(summary);

                // Details (children) go under the row
                RenderManagedRefChildrenInto(prop, underRowHost);
            }
            else
            {
                allowRowToggle = false;
                expandToggle.SetValueWithoutNotify(false);
                expandToggle.style.display = DisplayStyle.None;
                expandToggle.SetEnabled(false);

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

        private static void RenderManagedRefChildrenInto(SerializedProperty managedRefProp, VisualElement host)
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

                while (it.propertyPath != end.propertyPath && it.depth > parentDepth)
                {
                    // Draw children (not the parent)
                    var child = new PropertyField(it.Copy(), ObjectNames.NicifyVariableName(it.name));
                    child.AddToClassList("tsf__child-field");
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