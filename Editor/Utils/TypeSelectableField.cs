#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace Jungle.Editor
{
    /// <summary>
    /// Composite control with a Unity-like label on the left and an input row on the right.
    /// For managed refs, shows the current type name; if null, shows a muted "None".
    /// </summary>
    public sealed class TypeSelectableField : BindableElement
    {
        private readonly Label label; // left label (Unity style)
        private readonly VisualElement input; // right side "input" area
        private readonly VisualElement row; // your inline wrapper inside input
        private readonly VisualElement inputHost;
        private readonly VisualElement btns;
        private readonly Button addOrSwapBtn;
        private readonly Button clearBtn;

        private SerializedProperty prop;
        private Type baseType;
        private bool isManagedRef;

        public TypeSelectableField()
        {
            // Keep your theme class, add BaseField classes so labels behave like stock fields
            AddToClassList("jungle-editor");
            AddToClassList("unity-base-field");


            // Right input area
            input = new VisualElement();
            input.AddToClassList("unity-base-field__input");
            Add(input);

            // Your original row inside the input area
            row = new VisualElement();
            row.AddToClassList("jungle-section");
            row.AddToClassList("jungle-inline-wrapper");

            // Left label (works with label width settings)
            label = new Label();
            label.AddToClassList("unity-base-field__label");

            row.Add(label);


            input.Add(row);

            inputHost = new VisualElement();
            inputHost.style.flexGrow = 1;
            row.Add(inputHost);

            btns = new VisualElement();
            btns.AddToClassList("jungle-class-selector-inline-buttons");

            row.Add(btns);

            addOrSwapBtn = new Button(ShowMenu) { text = "+", tooltip = "Select or change type" };
            clearBtn = new Button(ClearValue) { text = "✕", tooltip = "Clear selection" };

            // Normalize casing ("Jungle-button" -> "jungle-button")
            addOrSwapBtn.AddToClassList("jungle-button");
            clearBtn.AddToClassList("jungle-button");
            clearBtn.style.fontSize = 10;

            btns.Add(addOrSwapBtn);
            btns.Add(clearBtn);
        }

        public void Initialize(SerializedProperty prop, Type baseType)
        {
            this.prop = prop;
            this.baseType = baseType;
            isManagedRef = prop.propertyType == SerializedPropertyType.ManagedReference;

            // Label shows the property name like normal Unity fields
            label.text = string.IsNullOrEmpty(prop.displayName) ? prop.name : prop.displayName;
            if (!string.IsNullOrEmpty(prop.tooltip)) label.tooltip = prop.tooltip;

            RebuildInput();
            RefreshButtons();

            // Keep UI in sync with serialized changes/rebinds
            this.TrackPropertyValue(this.prop, _ =>
            {
                RebuildInput();
                RefreshButtons();
            });
        }

        private void RebuildInput()
        {
            inputHost.Clear();

            if (isManagedRef)
            {
                var so = prop.serializedObject;
                so.Update();

                bool isEmpty = !prop.hasMultipleDifferentValues &&
                               prop.managedReferenceValue == null &&
                               string.IsNullOrEmpty(prop.managedReferenceFullTypename);

                if (prop.hasMultipleDifferentValues)
                {
                    var mixed = new Label("—");
                    mixed.AddToClassList("jungle-class-selector-field");
                    inputHost.Add(mixed);
                    return;
                }

                if (isEmpty)
                {
                    // Show muted "None" until a type is picked
                    var none = new Label("None");
                    none.AddToClassList("jungle-class-selector-field");
                    none.AddToClassList("jungle-empty-value"); // style this in USS
                    none.tooltip = "No value assigned";
                    inputHost.Add(none);
                    return;
                }

                // Value present → render full drawer
                CreateManagedRefDrawer(prop, inputHost);
            }
            else
            {
                // UnityEngine.Object branch
                var so = prop.serializedObject;
                so.Update();

                if (prop.hasMultipleDifferentValues)
                {
                    var mixed = new Label("—");
                    mixed.AddToClassList("jungle-class-selector-field");
                    inputHost.Add(mixed);
                    return;
                }

                var obj = prop.objectReferenceValue;
                if (obj == null)
                {
                    // Normal ObjectField when empty
                    var of = new ObjectField { objectType = baseType, allowSceneObjects = true };
                    of.BindProperty(prop);
                    of.AddToClassList("jungle-class-selector-field");
                    inputHost.Add(of);
                }
                else
                {
                    // Value present → embed its inspector (drawer/uxml)
                    CreateObjectRefDrawer(prop, inputHost);
                }
            }
        }

        private static void CreateManagedRefDrawer(SerializedProperty managedRefProp, VisualElement host)
        {
            // We want the property's drawer & its children, not another label (we already draw the left label)
            var pf = new PropertyField(managedRefProp, string.Empty);
            // Hide the base-field label column so it doesn’t steal left-space:
            pf.AddToClassList("jungle-inline-property-field");
            // Bind to the same serialized object
            pf.Bind(managedRefProp.serializedObject);
            host.Add(pf);
        }

        private static void CreateObjectRefDrawer(SerializedProperty objectProp, VisualElement host)
        {
            var obj = objectProp.objectReferenceValue;
            if (obj == null) return;

            // Renders the actual custom editor/uxml for the object
            var inspector = new InspectorElement(obj);
            inspector.AddToClassList("jungle-inline-inspector");
            host.Add(inspector);
        }

        private void ShowMenu()
        {
            if (isManagedRef)
            {
                EditorUtils.ShowAddManagedReferenceTypeMenuAndCreate(baseType, prop);
                RebuildInput();
            }
            else
            {
                EditorUtils.ShowAddComponentTypeMenuAndCreate(baseType, prop);
            }

            RefreshButtons();
        }

        private void ClearValue()
        {
            var so = prop.serializedObject;
            Undo.RecordObjects(so.targetObjects, "Clear Selection");
            so.Update();

            if (isManagedRef) prop.managedReferenceValue = null;
            else prop.objectReferenceValue = null;

            so.ApplyModifiedProperties();
            foreach (var t in so.targetObjects) EditorUtility.SetDirty(t);

            RebuildInput();
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            var so = prop.serializedObject;
            so.Update();

            bool hasValue = prop.hasMultipleDifferentValues || (isManagedRef
                ? (prop.managedReferenceValue != null || !string.IsNullOrEmpty(prop.managedReferenceFullTypename))
                : prop.objectReferenceValue != null);

            addOrSwapBtn.text = hasValue ? "⇄" : "+";
            clearBtn.style.display = hasValue ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private static Type ParseFullTypeName(string full)
        {
            if (string.IsNullOrEmpty(full)) return null;
            // "AssemblyName Full.Namespace.Type"
            int space = full.LastIndexOf(' ');
            string typeName = space >= 0 ? full[(space + 1)..] : full;
            return Type.GetType(typeName);
        }
    }
}
#endif