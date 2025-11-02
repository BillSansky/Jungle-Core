using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Jungle.Editor;
using Jungle.Values;
using Jungle.Values.Primitives;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Values.Editor
{
    [CustomPropertyDrawer(typeof(ClassMembersValue<>),true)]
    /// <summary>
    /// Property drawer that lets users pick component members matching the value
    /// type of <see cref="ClassMembersValue{T}"/> wrappers.
    /// </summary>
    public class MethodInvokerValueDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            
            var container = new VisualElement();

            var componentProp = property.FindPropertyRelative("component");
            var memberNameProp = property.FindPropertyRelative("memberName")
                                 ?? property.FindPropertyRelative("methodName");
            var lookupModeProp = property.FindPropertyRelative("lookupMode");

            if (memberNameProp == null)
            {
                Debug.LogError("MethodInvokerValueDrawer: Unable to find 'memberName' or legacy 'methodName' property.");
                return container;
            }

            // Create component field with class selection support
            var componentField = new PropertyField(componentProp, "Component");
            PropertyField lookupModeField = null;
            if (lookupModeProp != null && lookupModeProp.propertyType == SerializedPropertyType.Enum)
            {
                lookupModeField = new PropertyField(lookupModeProp, "Lookup Mode");
            }

            // Dropdown for method/property names
            var methodDropdown = new DropdownField("Member", new List<string> { "Select a component first" }, 0);

            container.Add(componentField);
            if (lookupModeField != null)
            {
                container.Add(lookupModeField);
            }
            container.Add(methodDropdown);

            bool selectorInitialized = false;
            componentField.schedule.Execute(() =>
            {
                if (selectorInitialized || componentField.panel == null)
                {
                    return;
                }

                selectorInitialized = true;
                EditorUtils.SetupFieldWithClassSelectionButton(componentField, typeof(IComponentReference), componentProp, null);
            });

            // Initial dropdown population
            UpdateMethodDropdown(property, componentProp, memberNameProp, methodDropdown);

            Component lastResolvedComponent = ResolveComponent(componentProp);

            // Update dropdown when component changes
            container.TrackPropertyValue(componentProp, _ =>
            {
                Component currentComponent = ResolveComponent(componentProp);
                bool componentChanged = currentComponent != lastResolvedComponent;

                UpdateMethodDropdown(property, componentProp, memberNameProp, methodDropdown);

                if (componentChanged)
                {
                    var serializedObject = memberNameProp.serializedObject;
                    serializedObject.Update();
                    memberNameProp.stringValue = string.Empty;
                    serializedObject.ApplyModifiedProperties();
                }

                lastResolvedComponent = currentComponent;
            });

            if (lookupModeProp != null)
            {
                container.TrackPropertyValue(lookupModeProp, _ =>
                {
                    UpdateMethodDropdown(property, componentProp, memberNameProp, methodDropdown);
                });
            }

            // Update property when dropdown changes
            methodDropdown.RegisterValueChangedCallback(evt =>
            {
                if (ResolveComponent(componentProp) != null && evt.newValue != "Select a component first" && !evt.newValue.StartsWith("No"))
                {
                    memberNameProp.stringValue = evt.newValue;
                    memberNameProp.serializedObject.ApplyModifiedProperties();
                }
            });
            
            return container;
        }

        private void UpdateMethodDropdown(SerializedProperty valueProperty, SerializedProperty componentProp, SerializedProperty memberNameProp, DropdownField dropdown)
        {
            Component component = ResolveComponent(componentProp);
            List<Type> acceptedReturnTypes = GetAcceptedReturnTypes(valueProperty);

            if (component == null)
            {
                dropdown.choices = new List<string> { "Select a component first" };
                dropdown.index = 0;
                return;
            }

            List<string> memberNames = GetAvailableMembers(component, acceptedReturnTypes);

            if (memberNames.Count == 0)
            {
                string typeDescription = acceptedReturnTypes.Count > 0
                    ? string.Join("/", acceptedReturnTypes.Select(t => t.Name).Distinct())
                    : "supported types";
                dropdown.choices = new List<string> { $"No members returning {typeDescription} found" };
                dropdown.index = 0;
                return;
            }

            dropdown.choices = memberNames;

            // Set current value
            string currentMethod = memberNameProp.stringValue;
            int currentIndex = memberNames.IndexOf(currentMethod);
            dropdown.index = currentIndex >= 0 ? currentIndex : 0;
            dropdown.value = dropdown.choices[dropdown.index];
        }

        private Component ResolveComponent(SerializedProperty componentProp)
        {
            if (componentProp == null)
            {
                return null;
            }

            if (componentProp.propertyType == SerializedPropertyType.ObjectReference)
            {
                return componentProp.objectReferenceValue as Component;
            }

            if (componentProp.propertyType == SerializedPropertyType.ManagedReference)
            {
                var reference = componentProp.managedReferenceValue as IComponentReference;
                if (reference != null)
                {
                    return reference.Component;
                }
            }

            return null;
        }

        private Type GetReturnTypeFromMethodInvokerType(Type type)
        {
            // Check if this type is or inherits from ClassMembersValue<T>
            Type currentType = type;
            while (currentType != null)
            {
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == typeof(ClassMembersValue<>))
                {
                    return currentType.GetGenericArguments()[0];
                }
                currentType = currentType.BaseType;
            }

            // If no generic type found, return void
            return typeof(void);
        }

        private List<string> GetAvailableMembers(Component component, IReadOnlyList<Type> acceptedReturnTypes)
        {
            if (component == null)
            {
                return new List<string>();
            }

            if (acceptedReturnTypes == null || acceptedReturnTypes.Count == 0)
            {
                return new List<string>();
            }

            Type type = component.GetType();

            bool Accepts(Type candidateType) => acceptedReturnTypes.Any(expected => expected.IsAssignableFrom(candidateType));

            var methodNames = type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => Accepts(m.ReturnType))
                .Where(m => m.GetParameters().Length == 0)
                .Where(m => !m.IsSpecialName)
                .Where(m => m.DeclaringType != typeof(Component) &&
                            m.DeclaringType != typeof(Behaviour) &&
                            m.DeclaringType != typeof(MonoBehaviour) &&
                            m.DeclaringType != typeof(UnityEngine.Object))
                .Select(m => m.Name)
                .ToList();

            var propertyNames = type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(p => Accepts(p.PropertyType))
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

        private Type GetMethodInvokerType(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                var managedRefValue = property.managedReferenceValue;
                if (managedRefValue != null)
                {
                    return managedRefValue.GetType();
                }
            }

            return fieldInfo.FieldType;
        }

        private bool IsComponentMemberValue(Type type)
        {
            return InheritsFromGenericType(type, typeof(ComponentClassMembersValue<>));
        }

        private bool InheritsFromGenericType(Type type, Type genericType)
        {
            Type current = type;
            while (current != null)
            {
                if (current.IsGenericType && current.GetGenericTypeDefinition() == genericType)
                {
                    return true;
                }

                current = current.BaseType;
            }

            return false;
        }

        private List<Type> GetAcceptedReturnTypes(SerializedProperty property)
        {
            Type invokerType = GetMethodInvokerType(property);
            Type expectedReturnType = GetReturnTypeFromMethodInvokerType(invokerType);

            var acceptedTypes = new List<Type>();

            if (expectedReturnType != typeof(void))
            {
                acceptedTypes.Add(expectedReturnType);
            }

            if (invokerType != null && IsComponentMemberValue(invokerType))
            {
                var lookupModeProp = property.FindPropertyRelative("lookupMode");
                if (lookupModeProp != null && lookupModeProp.propertyType == SerializedPropertyType.Enum && lookupModeProp.enumValueIndex != 0)
                {
                    acceptedTypes.Add(typeof(Component));
                    acceptedTypes.Add(typeof(GameObject));
                }
            }

            return acceptedTypes
                .Where(t => t != null)
                .Distinct()
                .ToList();
        }
    }
}
