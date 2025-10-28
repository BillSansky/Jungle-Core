using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Values.Editor
{
    [CustomPropertyDrawer(typeof(MethodInvokerValue<>))]
    public class MethodInvokerValueDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();

            var componentProp = property.FindPropertyRelative("component");
            var methodNameProp = property.FindPropertyRelative("methodName");

            // Get the generic type parameter T from MethodInvokerValue<T>
            Type returnType = GetExpectedReturnType();

            // Create component field
            var componentField = new PropertyField(componentProp, "Ref");
            container.Add(componentField);

            // Create method dropdown
            var methodDropdown = new DropdownField("Method");
            methodDropdown.choices = new List<string> { "Select a component first" };
            methodDropdown.value = "Select a component first";
            methodDropdown.SetEnabled(false);
            container.Add(methodDropdown);

            // Update dropdown when component changes
            componentField.RegisterValueChangeCallback(evt =>
            {
                UpdateMethodDropdown(componentProp, methodNameProp, methodDropdown, returnType);
                methodNameProp.stringValue = string.Empty;
                methodNameProp.serializedObject.ApplyModifiedProperties();
            });

            // Handle dropdown selection
            methodDropdown.RegisterValueChangedCallback(evt =>
            {
                if (componentProp.objectReferenceValue != null && evt.newValue != "Select a component first" && !evt.newValue.StartsWith("No"))
                {
                    methodNameProp.stringValue = evt.newValue;
                    methodNameProp.serializedObject.ApplyModifiedProperties();
                }
            });

            // Initialize dropdown
            container.schedule.Execute(() =>
            {
                UpdateMethodDropdown(componentProp, methodNameProp, methodDropdown, returnType);
            });

            return container;
        }

        private Type GetExpectedReturnType()
        {
            // fieldInfo.FieldType is MethodInvokerValue<T>
            Type fieldType = fieldInfo.FieldType;

            if (fieldType.IsGenericType)
            {
                return fieldType.GetGenericArguments()[0];
            }

            return typeof(void);
        }

        private void UpdateMethodDropdown(SerializedProperty componentProp, SerializedProperty methodNameProp, DropdownField dropdown, Type expectedReturnType)
        {
            Component component = componentProp.objectReferenceValue as Component;

            if (component != null)
            {
                List<string> methodNames = GetAvailableMethods(component, expectedReturnType);

                if (methodNames.Count > 0)
                {
                    dropdown.choices = methodNames;
                    dropdown.SetEnabled(true);

                    // Set current value if valid
                    if (methodNames.Contains(methodNameProp.stringValue))
                    {
                        dropdown.value = methodNameProp.stringValue;
                    }
                    else
                    {
                        dropdown.value = methodNames[0];
                    }
                }
                else
                {
                    dropdown.choices = new List<string> { $"No parameterless methods returning {expectedReturnType.Name} found" };
                    dropdown.value = $"No parameterless methods returning {expectedReturnType.Name} found";
                    dropdown.SetEnabled(false);
                }
            }
            else
            {
                dropdown.choices = new List<string> { "Select a component first" };
                dropdown.value = "Select a component first";
                dropdown.SetEnabled(false);
            }
        }

        private List<string> GetAvailableMethods(Component component, Type expectedReturnType)
        {
            if (component == null)
                return new List<string>();

            Type componentType = component.GetType();

            var methods = componentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.ReturnType == expectedReturnType)
                .Where(m => m.GetParameters().Length == 0)
                .Where(m => !m.IsSpecialName)
                .Where(m => m.DeclaringType != typeof(Component) &&
                           m.DeclaringType != typeof(Behaviour) &&
                           m.DeclaringType != typeof(MonoBehaviour) &&
                           m.DeclaringType != typeof(UnityEngine.Object))
                .Select(m => m.Name)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            return methods;
        }
    }
}
