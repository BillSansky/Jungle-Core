#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    public static class EditorUtils
    {
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
                    "Action", "Component", "Behaviour", "MonoBehaviour", "ScriptableObject", "Controller", "Manager",
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

        public static void SetupFieldWithClassSelectionButton(PropertyField propertyField, System.Type baseType,
            SerializedProperty property)
        {
            if (propertyField == null) return;

            // Create a container to hold both the PropertyField and the plus button
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;
            container.style.alignItems = Align.Center;

            var supportsManagedReferences = property.propertyType == SerializedPropertyType.ManagedReference;
            var supportsComponentReferences = baseType != null && typeof(Component).IsAssignableFrom(baseType);

            // Get the parent and index of the original PropertyField
            var parent = propertyField.parent;
            var index = parent.IndexOf(propertyField);

            // Remove the PropertyField from its current parent
            parent.Remove(propertyField);

            // Configure the PropertyField to grow and fill available space
            propertyField.style.flexGrow = 1;

            // Create the purple plus button
            var addButton = new Button();
            addButton.text = "+";
            addButton.AddToClassList("octoputs-add-inline-button");

            // Setup button click handler
            addButton.clicked += () =>
            {
                // Calculate button position in screen coordinates
                var buttonRect = addButton.worldBound;
                var buttonPosition = new Vector2(buttonRect.x, buttonRect.y + buttonRect.height);

                if (supportsManagedReferences)
                {
                    ShowAddManagedReferenceTypeMenuAndCreate(baseType, property);
                    return;
                }

                if (supportsComponentReferences)
                {
                    ShowAddComponentTypeMenuAndCreate(baseType, property);
                }
            };

            // Add both elements to the container
            container.Add(propertyField);
            container.Add(addButton);

            // Insert the container at the original PropertyField's position
            parent.Insert(index, container);
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

            ClassSelectionPopup.Show(screenPos, types, callback, menuTitle);
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
            string undoName = "Add Component")
        {
            if (!componentType.IsSubclassOf(typeof(Component)))
            {
                Debug.LogError(
                    $"Type {componentType.Name} is not a Component type and cannot be added to a GameObject.");
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