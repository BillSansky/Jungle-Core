#if UNITY_EDITOR
using System;
using System.Linq;
using Jungle.Editor;
using Jungle.Values;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Core.Editor
{
    [CustomPropertyDrawer(typeof(ValueReference<>), true)]
    public class ValueReferencePropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var valueSourceProperty = property.FindPropertyRelative("valueSource");
            var container = new VisualElement();

            if (valueSourceProperty == null)
            {
                container.Add(new Label("valueSource field not found"));
                return container;
            }

            var valueType = ResolveValueType();
            EnsureValueSourceInitialized(valueSourceProperty, valueType);

            var row = new VisualElement();
            row.style.flexDirection = FlexDirection.Row;
            row.style.alignItems = Align.Center;

            var valueField = new PropertyField(valueSourceProperty, property.displayName)
            {
                style = { flexGrow = 1f }
            };
            valueField.Bind(property.serializedObject);

            var addButton = new Button();
            addButton.text = "+";
            addButton.AddToClassList("octoputs-add-inline-button");
            addButton.clicked += () => ShowTypeSelection(addButton, valueSourceProperty, valueType);

            row.Add(valueField);
            row.Add(addButton);

            container.Add(row);
            return container;
        }

        private void ShowTypeSelection(Button button, SerializedProperty valueSourceProperty, Type valueType)
        {
            var baseType = typeof(ValueSource<>).MakeGenericType(valueType);
            var buttonRect = button.worldBound;
            var buttonPosition = new Vector2(buttonRect.x, buttonRect.y + buttonRect.height);

            EditorUtils.ShowAddTypeMenu(
                baseType,
                selectedType => AssignValueSource(valueSourceProperty, selectedType, valueType),
                buttonPosition,
                $"Select {EditorUtils.FormatTypeName(baseType)}"
            );
        }

        private void AssignValueSource(SerializedProperty property, Type selectedType, Type valueType)
        {
            if (selectedType == null)
            {
                // Selection was cancelled by the user, so keep the current value source.
                return;
            }

            if (!typeof(ValueSource<>).MakeGenericType(valueType).IsAssignableFrom(selectedType))
            {
                Debug.LogError($"Selected type {selectedType} is not a valid ValueSource for {valueType}.");
                return;
            }

            property.serializedObject.Update();
            property.managedReferenceValue = Activator.CreateInstance(selectedType);
            property.serializedObject.ApplyModifiedProperties();
        }

        private void EnsureValueSourceInitialized(SerializedProperty property, Type valueType)
        {
            if (property.managedReferenceValue != null)
            {
                return;
            }

            property.serializedObject.Update();
            var defaultType = typeof(LocalValue<>).MakeGenericType(valueType);
            property.managedReferenceValue = Activator.CreateInstance(defaultType);
            property.serializedObject.ApplyModifiedProperties();
        }

        private Type ResolveValueType()
        {
            var referenceType = FindValueReferenceType(fieldInfo.FieldType);
            return referenceType?.GetGenericArguments().FirstOrDefault() ?? typeof(object);
        }

        private static Type FindValueReferenceType(Type type)
        {
            if (type == null)
            {
                return null;
            }

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ValueReference<>))
            {
                return type;
            }

            if (type.IsArray)
            {
                return FindValueReferenceType(type.GetElementType());
            }

            if (type.IsGenericType)
            {
                foreach (var argument in type.GetGenericArguments())
                {
                    var candidate = FindValueReferenceType(argument);
                    if (candidate != null)
                    {
                        return candidate;
                    }
                }
            }

            return null;
        }
    }
}
#endif
