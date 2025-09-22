using System;
using System.Reflection;
using UnityEditor;
using UnityEngine.UIElements;
using Jungle.Attributes;

namespace Jungle.Editor
{
    /// <summary>
    /// Processes JungleListAttribute annotations and automatically creates appropriate list UIs
    /// </summary>
    public static class CustomListProcessor
    {
        /// <summary>
        /// Automatically processes all JungleListAttribute fields on the target object and creates appropriate UIs
        /// </summary>
        /// <param name="root">Root UI element</param>
        /// <param name="serializedObject">Serialized object containing the fields</param>
        public static void ProcessCustomLists(VisualElement root, SerializedObject serializedObject)
        {
            var targetType = serializedObject.targetObject.GetType();
            var fields = targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                var customListAttr = field.GetCustomAttribute<JungleListAttribute>();
                if (customListAttr == null) continue;

                // Get the serialized property
                var property = serializedObject.FindProperty(field.Name);
                if (property == null) continue;

                // Determine UI element name
                string uiElementName = customListAttr.UIElementName ?? GenerateUIElementName(field.Name);
                
                // Find the UI element
                var containerElement = root.Q<VisualElement>(uiElementName);
                if (containerElement == null)
                {
                    UnityEngine.Debug.LogWarning($"Could not find UI element '{uiElementName}' for field '{field.Name}'");
                    continue;
                }

                // Clear existing content
                containerElement.Clear();

                // Determine the generic type parameter
                Type listElementType = GetListElementType(field.FieldType);
                if (listElementType == null)
                {
                    UnityEngine.Debug.LogWarning($"Could not determine list element type for field '{field.Name}'");
                    continue;
                }

                CustomGenericListDrawer.CreateCustomList(listElementType,containerElement,
                    property,
                    serializedObject,
                    customListAttr.ListTitle,
                    customListAttr.EmptyMessage);
              
            }
        }

        /// <summary>
        /// Generates a UI element name from a field name (converts camelCase to kebab-case-content)
        /// </summary>
        private static string GenerateUIElementName(string fieldName)
        {
            // Convert camelCase to kebab-case and add "-content" suffix
            var result = "";
            for (int i = 0; i < fieldName.Length; i++)
            {
                if (i > 0 && char.IsUpper(fieldName[i]))
                {
                    result += "-";
                }
                result += char.ToLower(fieldName[i]);
            }
            return result + "-content";
        }

        /// <summary>
        /// Gets the element type from a List<T> field type
        /// </summary>
        private static Type GetListElementType(Type fieldType)
        {
            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
            {
                return fieldType.GetGenericArguments()[0];
            }
            return null;
        }

        // /// <summary>
        // /// Calls the generic CustomGenericListDrawer.CreateCustomList method using reflection
        // /// </summary>
        // private static void CallGenericCreateCustomList(
        //     VisualElement container, 
        //     SerializedProperty property, 
        //     SerializedObject serializedObject, 
        //     Type elementType,
        //     string title,
        //     string emptyMessage)
        // {
        //     var method = typeof(CustomGenericListDrawer).GetMethod("CreateCustomList", BindingFlags.Public | BindingFlags.Static);
        //     var genericMethod = method.MakeGenericMethod(elementType);
        //     
        //     genericMethod.Invoke(null, new object[] 
        //     { 
        //         container, 
        //         property, 
        //         serializedObject, 
        //         title, 
        //         emptyMessage 
        //     });
        // }
    }
}
