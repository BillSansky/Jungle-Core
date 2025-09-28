#if UNITY_EDITOR
using System;
using UnityEditor; 
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    /// <summary>
    /// Same row layout you had (label | summary | buttons). New: a separate toggle on the far left
    /// controls visibility of a details container placed UNDER the row. Buttons stay on the row.
    /// For ManagedReference: summary shows type; details show child properties in the under-row host.
    /// For UnityEngine.Object: behaves like before (ObjectField or inline Inspector); toggle is independent.
    /// </summary>
    public sealed class TypeSelectableField : BindableElement
    {
        // --- UI ---
        private readonly VisualElement input;     // root host
        private readonly VisualElement row;       // label | summary | btns
        private readonly Toggle expandToggle;     // separated "foldout" toggle (arrow)
        private readonly Label label;             // left label
        private readonly VisualElement content;   // right-side summary ("None" / type name / object field / inline inspector)
        private readonly VisualElement btns;      // buttons container
        private readonly Button btnPickOrSwap;    // "+" (pick) or "↺" (swap)
        private readonly Button btnClear;         // "✕"

        // Under-row details host (the "folded" section)
        private readonly VisualElement underRowHost;

        // --- Binding state ---
        private SerializedProperty prop;
        private Type baseType;
        private bool isManagedRef;

        // Avoid double-registering the toggle callback on rebind
        private EventCallback<ChangeEvent<bool>> expandToggleHandler;

        // Guard in case some drawer mistakenly applies to children
        [ThreadStatic] private static bool renderingChildren;

        public TypeSelectableField()
        {
            AddToClassList("jungle-editor");


            input = new VisualElement();
            Add(input);
            
            row = new VisualElement();
            row.AddToClassList("jungle-section");
            row.AddToClassList("jungle-inline-wrapper");
            
            expandToggle = new Toggle { value = false, text = "", focusable = false, tooltip = "Show details" };
            expandToggle.AddToClassList("tsf__toggle");
            expandToggle.AddToClassList("unity-foldout__toggle");
            row.Add(expandToggle); // first in row

            // Label next to the toggle
            label = new Label();
            label.AddToClassList("unity-base-field__label");
            row.Add(label);

            // Right content host (summary)
            content = new VisualElement { name = "tsf-content" };
            content.AddToClassList("jungle-inline-content");
            row.Add(content);

            // Buttons on the far right (keep your class)
            btns = new VisualElement();
            btns.AddToClassList("jungle-inline-button-group");
            row.Add(btns);

            btnPickOrSwap = new Button(OnPickOrSwapClicked) { text = "+" };
            btnPickOrSwap.tooltip = "Pick or change the type";
            btnPickOrSwap.AddToClassList("jungle-button");
            btns.Add(btnPickOrSwap);

            btnClear = new Button(OnClearClicked) { text = "✕" };
            btnClear.tooltip = "Clear";
            btnClear.AddToClassList("jungle-button");
            btns.Add(btnClear);

            input.Add(row);


            underRowHost = new VisualElement();
            underRowHost.AddToClassList("jungle-foldout-group");
            input.Add(underRowHost);
            

            row.RegisterCallback<ClickEvent>(evt =>
            {

                if (evt.target is VisualElement ve && (btns.Contains(ve) || ve == btnPickOrSwap || ve == btnClear))
                    return;

                expandToggle.value = !expandToggle.value;
                evt.StopPropagation();
            });


            btnPickOrSwap.RegisterCallback<ClickEvent>(evt => evt.StopPropagation());
            btnClear.RegisterCallback<ClickEvent>(evt => evt.StopPropagation());

        }

        /// <summary>For compatibility with older callsites.</summary>
        public void Initialize(SerializedProperty property, Type baseType) => Bind(property, baseType);

        /// <summary>Bind the field to a property and base type.</summary>
        public void Bind(SerializedProperty property, Type baseType)
        {
            prop = property ?? throw new ArgumentNullException(nameof(property));
            this.baseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
            isManagedRef = prop.propertyType == SerializedPropertyType.ManagedReference;

            bindingPath = prop.propertyPath;

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
                // Searchable popup anchored to the button
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
                        try { prop.managedReferenceValue = Activator.CreateInstance(t); }
                        catch (Exception e) { Debug.LogException(e); }
                    }
                    so.ApplyModifiedProperties();

                    // Repaint next tick to avoid flicker if Inspector rebuilds
                    schedule.Execute(RepaintContentOnly);
                    schedule.Execute(RefreshButtons);
                });
            }
            else
            {
                // For UnityEngine.Object, '+' doesn't pick a type; ensure UI stays fresh.
                RepaintContentOnly();
            }
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

            btnPickOrSwap.text = isManagedRef && hasValue ? "↺" : "+";
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

                // Show/hide foldout arrow based on selection
                expandToggle.style.display = empty ? DisplayStyle.None : DisplayStyle.Flex;

                if (empty)
                {
                    // Force the under-row area hidden if nothing selected,
                    // regardless of any previously saved expanded state.
                    underRowHost.style.display = DisplayStyle.None;

                    // Show "None" in-row; no details to render
                    var none = new Label("None");
                    none.AddToClassList("jungle-empty-value");
                    none.tooltip = "No value assigned";
                    content.Add(none);
                    return;
                }

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
                // UnityEngine.Object behavior: show ObjectField when null, inline Inspector when assigned
                var obj = prop.objectReferenceValue;

                // Show/hide foldout arrow based on selection
                bool hasObj = obj != null;
                expandToggle.style.display = hasObj ? DisplayStyle.Flex : DisplayStyle.None;
                if (!hasObj)
                    underRowHost.style.display = DisplayStyle.None;

                if (obj == null)
                {
                    var of = new ObjectField { objectType = baseType, allowSceneObjects = true };
                    of.BindProperty(prop);
                    content.Add(of);
                }
                else
                {
                    var inspector = new InspectorElement(obj);
                    inspector.AddToClassList("jungle-inline-inspector");
                    content.Add(inspector);
                }
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
