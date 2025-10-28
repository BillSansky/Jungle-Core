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
            var memberDropdown = new DropdownField("Member");
            memberDropdown.choices = new List<string> { "Select a component first" };
            memberDropdown.value = "Select a component first";
            memberDropdown.SetEnabled(false);
            container.Add(memberDropdown);

            // Update dropdown when component changes
            componentField.RegisterValueChangeCallback(evt =>
            {
                UpdateMemberDropdown(componentProp, methodNameProp, memberDropdown, returnType);
                methodNameProp.stringValue = string.Empty;
                methodNameProp.serializedObject.ApplyModifiedProperties();
            });

            // Handle dropdown selection
            memberDropdown.RegisterValueChangedCallback(evt =>
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
                UpdateMemberDropdown(componentProp, methodNameProp, memberDropdown, returnType);
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

        private void UpdateMemberDropdown(SerializedProperty componentProp, SerializedProperty methodNameProp, DropdownField dropdown, Type expectedReturnType)
        {
            Component component = componentProp.objectReferenceValue as Component;

            if (component != null)
            {
                List<string> memberNames = GetAvailableMembers(component, expectedReturnType);

                if (memberNames.Count > 0)
                {
                    dropdown.choices = memberNames;
                    dropdown.SetEnabled(true);

                    // Set current value if valid
                    if (memberNames.Contains(methodNameProp.stringValue))
                    {
                        dropdown.value = methodNameProp.stringValue;
                    }
                    else
                    {
                        dropdown.value = memberNames[0];
                    }
                }
                else
                {
                    dropdown.choices = new List<string> { $"No parameterless members returning {expectedReturnType.Name} found" };
                    dropdown.value = $"No parameterless members returning {expectedReturnType.Name} found";
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

        private List<string> GetAvailableMembers(Component component, Type expectedReturnType)
        {
            if (component == null)
                return new List<string>();

            Type componentType = component.GetType();

            var methodNames = componentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.ReturnType == expectedReturnType)
                .Where(m => m.GetParameters().Length == 0)
                .Where(m => !m.IsSpecialName)
                .Where(m => m.DeclaringType != typeof(Component) &&
                           m.DeclaringType != typeof(Behaviour) &&
                           m.DeclaringType != typeof(MonoBehaviour) &&
                           m.DeclaringType != typeof(UnityEngine.Object))
                .Select(m => m.Name)
                .ToList();

            var propertyNames = componentType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => p.PropertyType == expectedReturnType)
                .Where(p => p.GetIndexParameters().Length == 0)
                .Where(p =>
                {
                    MethodInfo getter = p.GetGetMethod(true);
                    return getter != null && getter.GetParameters().Length == 0;
                })
                .Select(p => p.Name)
                .ToList();

            return methodNames
                .Concat(propertyNames)
                .Distinct()
                .OrderBy(name => name)
                .ToList();
        }
    }
}
