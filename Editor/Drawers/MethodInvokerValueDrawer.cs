using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Jungle.Values.Editor
{
    [CustomPropertyDrawer(typeof(MethodInvokerValue<>))]
    public class MethodInvokerValueDrawer : PropertyDrawer
    {
        private const float LineHeight = 18f;
        private const float Spacing = 2f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var componentProp = property.FindPropertyRelative("component");
            var methodNameProp = property.FindPropertyRelative("methodName");

            Rect componentRect = new Rect(position.x, position.y, position.width, LineHeight);
            Rect methodRect = new Rect(position.x, position.y + LineHeight + Spacing, position.width, LineHeight);

            // Draw component field
            EditorGUI.BeginChangeCheck();
            EditorGUI.PropertyField(componentRect, componentProp, new GUIContent("Component"));
            if (EditorGUI.EndChangeCheck())
            {
                methodNameProp.stringValue = string.Empty;
            }

            // Draw method dropdown
            Component component = componentProp.objectReferenceValue as Component;
            if (component != null)
            {
                List<string> methodNames = GetAvailableMethods(component);

                if (methodNames.Count > 0)
                {
                    int currentIndex = methodNames.IndexOf(methodNameProp.stringValue);
                    if (currentIndex < 0) currentIndex = 0;

                    EditorGUI.BeginChangeCheck();
                    int newIndex = EditorGUI.Popup(methodRect, "Method", currentIndex, methodNames.ToArray());
                    if (EditorGUI.EndChangeCheck())
                    {
                        methodNameProp.stringValue = methodNames[newIndex];
                    }
                }
                else
                {
                    EditorGUI.LabelField(methodRect, "Method", "No parameterless void methods found");
                }
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUI.Popup(methodRect, "Method", 0, new string[] { "Select a component first" });
                EditorGUI.EndDisabledGroup();
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (LineHeight + Spacing) * 2;
        }

        private List<string> GetAvailableMethods(Component component)
        {
            if (component == null)
                return new List<string>();

            Type componentType = component.GetType();

            var methods = componentType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(m => m.ReturnType == typeof(void))
                .Where(m => m.GetParameters().Length == 0)
                .Where(m => !m.IsSpecialName) // Exclude property getters/setters
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
