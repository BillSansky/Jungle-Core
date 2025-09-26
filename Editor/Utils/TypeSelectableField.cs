#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace Jungle.Editor
{
    /// <summary>
    /// Composite control that hosts the actual input (ObjectField OR a compact ManagedRef display)
    /// and the (+ / ⇄ / ✕) actions in one stable row—no reparenting of PropertyField internals.
    /// </summary>
    public sealed class TypeSelectableField : BindableElement
    {
        private readonly VisualElement row;
        private readonly VisualElement inputHost;
        private readonly VisualElement btns;
        private readonly Button addOrSwapBtn;
        private readonly Button clearBtn;

        private SerializedProperty prop;
        private Type baseType;
        private bool isManagedRef;

        public TypeSelectableField()
        {
            AddToClassList("jungle-class-selector-container");

            row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;
            Add(row);

            inputHost = new VisualElement();
            inputHost.style.flexGrow = 1;
            row.Add(inputHost);

            btns = new VisualElement();
            btns.AddToClassList("jungle-class-selector-inline-buttons");
            btns.style.flexDirection = FlexDirection.Row;
            btns.style.alignItems = Align.Center;
            row.Add(btns);

            addOrSwapBtn = new Button(() => ShowMenu()) { text = "+", tooltip = "Select or change type" };
            clearBtn     = new Button(ClearValue)       { text = "✕", tooltip = "Clear selection" };

            btns.Add(addOrSwapBtn);
            btns.Add(clearBtn);
        }

        public void Initialize(SerializedProperty prop, Type baseType)
        {
            this.prop = prop;
            this.baseType = baseType;
            isManagedRef = prop.propertyType == SerializedPropertyType.ManagedReference;

            RebuildInput();
            RefreshButtons();

            // Keep button state in sync with serialized changes/rebinds
            this.TrackPropertyValue(this.prop, _ => RefreshButtons());
        }

        private void RebuildInput()
        {
            inputHost.Clear();

            if (isManagedRef)
            {
                // Compact managed-ref display; you can style this with USS
                var label = new Label(GetManagedRefNiceName());
                label.AddToClassList("jungle-class-selector-field");
                inputHost.Add(label);
            }
            else
            {
                var of = new ObjectField { objectType = baseType, allowSceneObjects = true };
                of.BindProperty(prop); // safe public API binding
                of.AddToClassList("jungle-class-selector-field");
                inputHost.Add(of);
            }
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

            if (isManagedRef) RebuildInput();
            RefreshButtons();
        }

        private void RefreshButtons()
        {
            var so = prop.serializedObject;
            so.Update();

            bool hasValue = isManagedRef
                ? !string.IsNullOrEmpty(prop.managedReferenceFullTypename)
                : prop.objectReferenceValue != null;

            addOrSwapBtn.text = hasValue ? "⇄" : "+";
            clearBtn.style.display = hasValue ? DisplayStyle.Flex : DisplayStyle.None;
        }

        private string GetManagedRefNiceName()
        {
            var full = prop.managedReferenceFullTypename;
            if (string.IsNullOrEmpty(full)) return "None";
            var i = full.LastIndexOf('.');
            return i >= 0 ? full[(i + 1)..] : full;
        }
    }
}
#endif
