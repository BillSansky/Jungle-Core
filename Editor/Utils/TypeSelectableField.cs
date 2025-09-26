#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;   // AdvancedDropdown
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    /// <summary>
    /// Wrapper field with a Unity-like left label + buttons on the right.
    /// The wrapper ALWAYS stays. The selected class (managed ref) renders INSIDE the right pane.
    /// For UnityEngine.Object, we show ObjectField when null, and an inline Inspector when assigned.
    /// </summary>
    public sealed class TypeSelectableField : BindableElement
    {
        // --- UI ---
        private readonly VisualElement input;     // unity-base-field__input
        private readonly VisualElement row;       // row container
        private readonly Label label;             // left label
        private readonly VisualElement content;   // right-side host where the chosen class renders
        private readonly VisualElement btns;      // buttons container
        private readonly Button btnPickOrSwap;    // "+" (pick) or "↺" (swap)
        private readonly Button btnClear;         // "✕"

        // --- Binding state ---
        private SerializedProperty prop;
        private Type baseType;
        private bool isManagedRef;

        // Guard in case some drawer mistakenly applies to children
        [ThreadStatic] private static bool renderingChildren;

        public TypeSelectableField()
        {
            // Match Unity’s base field so the label column width works as usual
           // AddToClassList("unity-base-field");
            AddToClassList("jungle-editor");
            
            
            input = new VisualElement();
           // input.AddToClassList("unity-base-field__input");
            Add(input);

            row = new VisualElement();
            row.AddToClassList("jungle-section");
            row.AddToClassList("jungle-inline-wrapper");
            
            input.Add(row);

            // Left label (controlled by --unity-base-field-label-width)
            label = new Label();
            label.AddToClassList("unity-base-field__label");
            row.Add(label);

            // Right content host (flex-grow so it takes the space)
            content = new VisualElement { name = "tsf-content" };
            content.AddToClassList("jungle-inline-content");
            row.Add(content);

            // Buttons on the far right
            btns = new VisualElement();
            btns.AddToClassList("jungle-class-selector-inline-buttons");
            
            row.Add(btns);

            btnPickOrSwap = new Button(OnPickOrSwapClicked) { text = "+" };
            btnPickOrSwap.tooltip = "Pick or change the type";
            btns.Add(btnPickOrSwap);

            btnClear = new Button(OnClearClicked) { text = "✕" };
            btnClear.tooltip = "Clear";
            btns.Add(btnClear);
        }

        /// <summary>
        /// Kept for compatibility with your previous pattern.
        /// </summary>
        public void Initialize(SerializedProperty property, Type baseType)
        {
            Bind(property, baseType);
        }

        /// <summary>
        /// Bind the field to a property and base type.
        /// </summary>
        public void Bind(SerializedProperty property, Type baseType)
        {
            prop = property ?? throw new ArgumentNullException(nameof(property));
            this.baseType = baseType ?? throw new ArgumentNullException(nameof(baseType));
            isManagedRef = prop.propertyType == SerializedPropertyType.ManagedReference;

            
            bindingPath = prop.propertyPath;
            
            label.text = string.IsNullOrEmpty(prop.displayName) ? prop.name : prop.displayName;
            label.tooltip = prop.tooltip;

            // First paint
            RepaintContentOnly();
            RefreshButtons();

            // Keep in sync with changes
            this.TrackPropertyValue(prop, _ =>
            {
                RepaintContentOnly();
                RefreshButtons();
            });

            // Also track the serialized object in case rebinds happen
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
                // Searchable, reliable popup anchored to the button
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

                    // Paint on next tick so if Unity rebuilds the inspector we don't flash-disappear
                    schedule.Execute(RepaintContentOnly);
                    schedule.Execute(RefreshButtons);
                });
            }
            else
            {
                // For UnityEngine.Object fields, '+' has nothing to "pick type" for.
                // We just ensure the ObjectField is visible/focused.
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
            if (prop == null) return;

            if (prop.hasMultipleDifferentValues)
            {
                content.Add(new Label("—") { name = "mixed" });
                return;
            }

            if (isManagedRef)
            {
                bool empty = prop.managedReferenceValue == null &&
                             string.IsNullOrEmpty(prop.managedReferenceFullTypename);

                if (empty)
                {
                    var none = new Label("None");
                    none.AddToClassList("jungle-empty-value");
                    none.tooltip = "No value assigned";
                    content.Add(none);
                    return;
                }

                // Draw ONLY the children of the selected class (no self-recursion)
                RenderManagedRefChildrenInto(prop, content);
            }
            else
            {
                var obj = prop.objectReferenceValue;
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
                    // IMPORTANT: draw children, not the parent
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

        // --------------- Type picker (searchable, reliable) ---------------

        private sealed class TypePickerDropdown : AdvancedDropdown
        {
            private readonly Type baseType;
            private readonly Action<Type> onPicked;

            private TypePickerDropdown(Type baseType, Action<Type> onPicked)
                : base(new AdvancedDropdownState())
            {
                this.baseType = baseType;
                this.onPicked = onPicked;
                minimumSize = new Vector2(300, 400);
            }

            public static void Show(Rect anchorWorldRect, Type baseType, Action<Type> onPicked)
            {
                var dd = new TypePickerDropdown(baseType, onPicked);
                dd.Show(anchorWorldRect); // works with UIElements worldBound rect
            }

            protected override AdvancedDropdownItem BuildRoot()
            {
                var root = new AdvancedDropdownItem(baseType.Name);

                // "None" entry
                root.AddChild(new TypeItem("(None)", null));

                // All concrete derived types with default ctor
                var types = TypeCache.GetTypesDerivedFrom(baseType)
                    .Where(t => !t.IsAbstract && !t.IsGenericType && t.GetConstructor(Type.EmptyTypes) != null)
                    .OrderBy(t => t.FullName)
                    .ToList();

                // Build a folder-like structure by namespace
                foreach (var t in types)
                {
                    var path = (t.FullName ?? t.Name).Split('.');
                    AdvancedDropdownItem parent = root;
                    for (int i = 0; i < path.Length - 1; i++)
                    {
                        var n = path[i];
                        var existing = parent.children.FirstOrDefault(c => c.name == n);
                        if (existing == null)
                        {
                            existing = new AdvancedDropdownItem(n);
                            parent.AddChild(existing);
                        }
                        parent = existing;
                    }

                    parent.AddChild(new TypeItem(path[^1], t));
                }

                return root;
            }

            protected override void ItemSelected(AdvancedDropdownItem item)
            {
                if (item is TypeItem ti)
                    onPicked?.Invoke(ti.Type);
            }

            private sealed class TypeItem : AdvancedDropdownItem
            {
                public readonly Type Type;
                public TypeItem(string name, Type type) : base(name) { Type = type; }
            }
        }
    }
}
#endif
