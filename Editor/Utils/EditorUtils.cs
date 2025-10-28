#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    public static class EditorUtils
    {
        public static Type ResolveElementType(Type overrideBase,
            Type fieldType,
            Type rootType,
            string propertyPath)
        {
            if (overrideBase != null) return overrideBase;
            
            var t = fieldType ?? GetFieldTypeFromPropertyPath(rootType, propertyPath);
            var elem = TryGetEnumerableElementType(t);
            if (elem != null) return elem;

 
            return typeof(object);
        }

        private static Type TryGetEnumerableElementType(Type t)
        {
            if (t == null) return null;

            // Arrays
            if (t.IsArray) return t.GetElementType();

            // Direct generic List<T>
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
                return t.GetGenericArguments()[0];

            // Check common generic interfaces
            foreach (var i in t.GetInterfaces())
            {
                if (!i.IsGenericType) continue;
                var g = i.GetGenericTypeDefinition();
                if (g == typeof(IList<>) ||
                    g == typeof(ICollection<>) ||
                    g == typeof(IEnumerable<>) ||
                    g == typeof(IReadOnlyList<>))
                {
                    return i.GetGenericArguments()[0];
                }
            }

            // If t itself is a generic with a single type arg (custom wrappers)
            if (t.IsGenericType && t.GetGenericArguments().Length == 1)
                return t.GetGenericArguments()[0];

            return t;
        }

        private static Type GetFieldTypeFromPropertyPath(Type rootType, string propertyPath)
        {
            const BindingFlags BF = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var t = rootType;
            FieldInfo last = null;

            foreach (var raw in propertyPath.Split('.'))
            {
                if (raw == "Array") continue;
                if (raw.StartsWith("data[")) continue;

                last = t.GetField(raw, BF);
                if (last == null) break;
                t = last.FieldType;
            }

            return last?.FieldType;
        }


        /// <summary>
        /// Gets all available types that inherit from a specified base type using TypeCache
        /// </summary>
        /// <typeparam name="T">The base type to search for subclasses of</typeparam>
        /// <returns>List of types that inherit from T and can be instantiated</returns>
        public static List<Type> GetAllSubclassTypes<T>() where T : class
        {
            return GetAllSubclassTypes(typeof(T));
        }

        public static List<Type> GetAllSubclassTypes(Type type)
        {
            // Use TypeCache instead of reflection for better performance
            var types = TypeCache.GetTypesDerivedFrom(type)
                .Where(t => !t.IsAbstract && t.IsClass)
                .OrderBy(t => t.Name)
                .ToList();

            return types;
        }


        /// <summary>
        /// Formats type names for display in menus by removing common suffixes and adding spaces
        /// </summary>
        /// <param name="type">The type to format</param>
        /// <param name="suffixesToRemove">Optional array of suffixes to remove from the type name. If null, uses common defaults.</param>
        /// <returns>Formatted display name</returns>
        public static string FormatTypeName(Type type, params string[] suffixesToRemove)
        {
            if (type == null)
                return string.Empty;

            var typeName = type.Name;

            // Use default suffixes if none provided
            if (suffixesToRemove == null || suffixesToRemove.Length == 0)
            {
                suffixesToRemove = new[]
                {
                    "Action", "Ref", "Behaviour", "MonoBehaviour", "ScriptableObject", "Controller", "Manager",
                    "Handler", "Service", "Provider"
                };
            }

            // Remove specified suffixes for cleaner display
            foreach (var suffix in suffixesToRemove)
            {
                if (typeName.EndsWith(suffix, StringComparison.Ordinal))
                {
                    typeName = typeName.Substring(0, typeName.Length - suffix.Length);
                    break; // Only remove the first matching suffix
                }
            }

            // Add spaces before capital letters for better readability
            typeName = System.Text.RegularExpressions.Regex.Replace(typeName, "([A-Z])", " $1").Trim();

            return typeName;
        }

        /// <summary>
        /// Formats action type names for display in menus by removing "Action" suffix and adding spaces
        /// </summary>
        /// <param name="actionType">The action type to format</param>
        /// <returns>Formatted display name</returns>
        public static string FormatActionTypeName(Type actionType)
        {
            return FormatTypeName(actionType, "Action");
        }

        private const string JungleEditorStyleSheetResource = "JungleEditorStyles";

        public static void SetupFieldWithClassSelectionButton(PropertyField propertyField, Type baseType,
            SerializedProperty property, System.Reflection.FieldInfo fieldInfo)
        {
            // Remove the original field from layout…
            var parent = propertyField.parent;
            var index = parent.IndexOf(propertyField);
            parent.Remove(propertyField);

            // …and insert our clean composite control in the same spot.
            var selector = new TypeSelectableField();
            selector.Bind(property, baseType, fieldInfo);

            // Optional: keep your stylesheet hookup
            AttachJungleEditorStyles(selector);

            parent.Insert(index, selector);
        }

        private static void AttachJungleEditorStyles(VisualElement element)
        {
            if (element == null)
            {
                return;
            }

            var styleSheet = Resources.Load<StyleSheet>(JungleEditorStyleSheetResource);
            if (styleSheet == null || element.styleSheets.Contains(styleSheet))
            {
                return;
            }

            element.styleSheets.Add(styleSheet);
        }


        public static void ShowAddTypeMenu(Type typeToShow, Action<Type> callback, Vector2? buttonPosition = null,
            string menuTitle = null)
        {
            var types = GetAllSubclassTypes(typeToShow);

            if (string.IsNullOrEmpty(menuTitle))
            {
                menuTitle = $"Select {FormatTypeName(typeToShow)}";
            }

            Vector2 screenPos;

            if (buttonPosition.HasValue)
            {
                // Use the provided button position if available
                screenPos = buttonPosition.Value;
            }
            else
            {
                // Fallback to mouse position for backwards compatibility
                screenPos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            }

            ClassSelectionWindow.Show(screenPos, types, callback, menuTitle);
        }

        public static void ShowAddComponentTypeMenuAndCreate(Type componentType,
            SerializedProperty property)
        {
            void CreateComponent(Type type)
            {
                CreateEditorComponentFieldValue(type, property);
            }

            ShowAddTypeMenu(componentType, CreateComponent);
        }

        public static void ShowAddManagedReferenceTypeMenuAndCreate(Type referenceType, SerializedProperty property)
        {
            void CreateReference(Type type)
            {
                CreateManagedReferenceFieldValue(type, property);
            }

            ShowAddTypeMenu(referenceType, CreateReference);
        }


        /// <summary>
        /// Shows a context menu with all available types that inherit from the specified base type
        /// </summary>
        /// <typeparam name="T">The base type to search for subclasses of</typeparam>
        /// <param name="onTypeSelected">Callback when a type is selected</param>
        /// <param name="buttonPosition">Optional explicit button position in screen coordinates</param>
        /// <param name="menuTitle">Optional title for the menu</param>
        public static void ShowAddTypeMenu<T>(Action<Type> onTypeSelected, Vector2? buttonPosition = null,
            string menuTitle = null) where T : class
        {
            ShowAddTypeMenu(typeof(T), onTypeSelected, buttonPosition, menuTitle);
        }


        /// <summary>
        /// Adds a component of the specified type to a GameObject and  adds it to a SerializedProperty 
        /// </summary>
        /// <param name="componentType">Type of component to add</param>
        /// <param name="targetGameObject">GameObject to add the component to</param>
        /// <param name="property">Optional SerializedProperty of the array to add the component to</param>
        /// <param name="serializedObject">Optional SerializedObject to apply changes to</param>
        /// <param name="undoName">Name for the undo operation</param>
        /// <returns>The created component</returns>
        public static Component CreateEditorComponentFieldValue(Type componentType, SerializedProperty property,
            GameObject targetGameObject = null,
            string undoName = "Add Ref")
        {
            if (!componentType.IsSubclassOf(typeof(Component)))
            {
                Debug.LogError(
                    $"Type {componentType.Name} is not a Ref type and cannot be added to a GameObject.");
                return null;
            }

            var serializedObject = property.serializedObject;

            var go = targetGameObject ?? ((Component)serializedObject.targetObject).gameObject;

            if (!go)
            {
                Debug.LogError("GameObject not found when attempting to add a component to a serialized field");
                return null;
            }

            var newComponent = go.AddComponent(componentType);

            Undo.RecordObject(serializedObject.targetObject, undoName);

            serializedObject.Update();

            property.objectReferenceValue = newComponent;

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(serializedObject.targetObject);

            return newComponent;
        }

        public static object CreateManagedReferenceFieldValue(Type referenceType, SerializedProperty property,
            string undoName = "Set Reference")
        {
            var serializedObject = property.serializedObject;

            Undo.RecordObject(serializedObject.targetObject, undoName);

            serializedObject.Update();

            var instance = Activator.CreateInstance(referenceType);
            property.managedReferenceValue = instance;

            serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty(serializedObject.targetObject);

            return instance;
        }
    }
}

#endif