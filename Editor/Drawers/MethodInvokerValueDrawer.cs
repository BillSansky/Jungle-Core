using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jungle.Values.Primitives;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Values.Editor
{
    [CustomPropertyDrawer(typeof(ClassMembersValue<>),true)]
    public class MethodInvokerValueDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            
            var container = new VisualElement();

            var componentProp = property.FindPropertyRelative("component");
            var methodNameProp = property.FindPropertyRelative("methodName");

            Type returnType = GetExpectedReturnType(property);

            // Create ObjectField
            var componentField = new ObjectField("Component")
            {
                objectType = typeof(Component),
                value = componentProp.objectReferenceValue
            };
            componentField.BindProperty(componentProp);

            // Dropdown for method/property names
            var methodDropdown = new DropdownField("Member", new List<string> { "Select a component first" }, 0);

            container.Add(componentField);
            container.Add(methodDropdown);
            

            // Initial dropdown population
            UpdateMethodDropdown(componentProp, methodNameProp, methodDropdown, returnType);

            // Update dropdown when component changes
            componentField.RegisterValueChangedCallback(evt =>
            {
                UpdateMethodDropdown(componentProp, methodNameProp, methodDropdown, returnType);
                methodNameProp.stringValue = string.Empty;
                methodNameProp.serializedObject.ApplyModifiedProperties();
            });

            // Update property when dropdown changes
            methodDropdown.RegisterValueChangedCallback(evt =>
            {
                if (componentProp.objectReferenceValue != null && evt.newValue != "Select a component first" && !evt.newValue.StartsWith("No"))
                {
                    methodNameProp.stringValue = evt.newValue;
                    methodNameProp.serializedObject.ApplyModifiedProperties();
                }
            });
            
            return container;
        }

        private void UpdateMethodDropdown(SerializedProperty componentProp, SerializedProperty methodNameProp, DropdownField dropdown, Type returnType)
        {
            Component component = componentProp.objectReferenceValue as Component;
            
            if (component == null)
            {
                dropdown.choices = new List<string> { "Select a component first" };
                dropdown.index = 0;
                return;
            }

            List<string> methodNames = GetAvailableMethods(component, returnType);
            
            if (methodNames.Count == 0)
            {
                dropdown.choices = new List<string> { $"No methods returning {returnType.Name} found" };
                dropdown.index = 0;
                return;
            }

            dropdown.choices = methodNames;
            
            // Set current value
            string currentMethod = methodNameProp.stringValue;
            int currentIndex = methodNames.IndexOf(currentMethod);
            dropdown.index = currentIndex >= 0 ? currentIndex : 0;
            dropdown.value = dropdown.choices[dropdown.index];
        }

        private Type GetExpectedReturnType(SerializedProperty property)
        {
            // Try to get the actual runtime type from the property
            // This works for SerializeReference fields
            
            // For SerializeReference, get the managed reference value
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                var managedRefValue = property.managedReferenceValue;
                if (managedRefValue != null)
                {
                    Type actualType = managedRefValue.GetType();
                    return GetReturnTypeFromMethodInvokerType(actualType);
                }
            }
            
            // Fall back to fieldInfo for direct field declarations
            Type fieldType = fieldInfo.FieldType;
            return GetReturnTypeFromMethodInvokerType(fieldType);
        }

        private Type GetReturnTypeFromMethodInvokerType(Type type)
        {
            // Check if this type is or inherits from ClassMembersValue<T>
            Type currentType = type;
            while (currentType != null)
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition().Name == "ClassMembersValue`1")
                {
                    return currentType.GetGenericArguments()[0];
                }
                currentType = currentType.BaseType;
            }

            // If no generic type found, return void
            return typeof(void);
        }

        private List<string> GetAvailableMethods(Component component, Type expectedReturnType)
        {
            if (component == null)
                return new List<string>();

            Type type = component.GetType();

            var methodNames = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.ReturnType == expectedReturnType)
                .Where(m => m.GetParameters().Length == 0)
                .Where(m => !m.IsSpecialName)
                .Where(m => m.DeclaringType != typeof(Component) &&
                            m.DeclaringType != typeof(Behaviour) &&
                            m.DeclaringType != typeof(MonoBehaviour) &&
                            m.DeclaringType != typeof(UnityEngine.Object))
                .Select(m => m.Name)
                .ToList();

            var propertyNames = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => p.PropertyType == expectedReturnType)
                .Where(p => p.GetIndexParameters().Length == 0)
                .Where(p =>
                {
                    MethodInfo getter = p.GetGetMethod(true);
                    return getter != null && getter.GetParameters().Length == 0;
                })
                .Select(p => p.Name)
                .ToList();

            return methodNames.Concat(propertyNames).Distinct().OrderBy(n => n).ToList();
        }
    }
}
