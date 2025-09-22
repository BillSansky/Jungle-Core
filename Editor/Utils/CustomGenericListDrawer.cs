#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Jungle.Editor
{
    /// <summary>
    /// Custom drawer for lists of managed reference objects (non-Unity Objects)
    /// Displays each element with class name as title and provides a plus button with contextual menu
    /// </summary>
    public static class CustomGenericListDrawer
    {
        // Cache for property drawer types to avoid repeated lookups
        private static Dictionary<Type, Type> propertyDrawerCache = new Dictionary<Type, Type>();
        private static bool cacheInitialized = false;

        // Cached reflection data to avoid repeated lookups
        private static FieldInfo customPropertyDrawerTypeField;
        private static FieldInfo propertyDrawerFieldInfoField;
        private static FieldInfo dummyFieldInfo;

        /// <summary>
        /// Initialize the property drawer cache using Unity's TypeCache for efficient type discovery
        /// </summary>
        private static void InitializePropertyDrawerCache()
        {
            if (cacheInitialized) return;

            propertyDrawerCache.Clear();

            // Cache reflection data once
            if (customPropertyDrawerTypeField == null)
            {
                customPropertyDrawerTypeField =
                    typeof(CustomPropertyDrawer).GetField("m_Type", BindingFlags.NonPublic | BindingFlags.Instance);
                propertyDrawerFieldInfoField =
                    typeof(PropertyDrawer).GetField("m_FieldInfo", BindingFlags.NonPublic | BindingFlags.Instance);
                dummyFieldInfo =
                    typeof(DummyDrawerClass).GetField("dummyField", BindingFlags.NonPublic | BindingFlags.Instance);
            }

            // Use TypeCache to efficiently get all types with CustomPropertyDrawer attribute
            var drawerTypes = TypeCache.GetTypesWithAttribute<CustomPropertyDrawer>();

            foreach (var drawerType in drawerTypes)
            {
                if (!drawerType.IsSubclassOf(typeof(PropertyDrawer)) || drawerType.IsAbstract)
                    continue;

                // Get all CustomPropertyDrawer attributes
                var attributes = drawerType.GetCustomAttributes<CustomPropertyDrawer>();

                foreach (var attr in attributes)
                {
                    // Use cached field info to get the target type from the attribute
                    if (customPropertyDrawerTypeField != null)
                    {
                        var targetType = customPropertyDrawerTypeField.GetValue(attr) as Type;
                        if (targetType != null)
                        {
                            propertyDrawerCache[targetType] = drawerType;
                        }
                    }
                }
            }

            cacheInitialized = true;
        }

        /// <summary>
        /// Try to get a custom property drawer for the specified type
        /// </summary>
        /// <param name="targetType">The type to find a property drawer for</param>
        /// <returns>Property drawer instance, or null if none found</returns>
        private static PropertyDrawer TryCreateCustomPropertyDrawer(Type targetType)
        {
            InitializePropertyDrawerCache();

            if (propertyDrawerCache.TryGetValue(targetType, out var drawerType))
            {
                try
                {
                    var drawer = Activator.CreateInstance(drawerType) as PropertyDrawer;

                    // Set the fieldInfo on the drawer if needed using cached reflection data
                    if (propertyDrawerFieldInfoField != null && dummyFieldInfo != null)
                    {
                        propertyDrawerFieldInfoField.SetValue(drawer, dummyFieldInfo);
                    }

                    return drawer;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Failed to create custom property drawer for {targetType.Name}: {ex.Message}");
                }
            }

            return null;
        }

           public static VisualElement CreateCustomList(Type type,VisualElement container,
            SerializedProperty listProperty,
            SerializedObject serializedObject,
            string listTitle = "List",
            string emptyListMessage = "No items in list") 
        {
            // Load the UXML template
            var template = Resources.Load<VisualTreeAsset>("CustomGenericListContainer");


            // Clone the template
            var listContainer = template.CloneTree();

            // Get references to elements
            var titleLabel = listContainer.Q<Label>("list-title");
            var addButton = listContainer.Q<Button>("add-button");
            addButton.AddToClassList("octoputs-add-inline-button");
            var contentContainer = listContainer.Q<VisualElement>("list-content");
            

            // Store the property path to re-obtain fresh property references
            var propertyPath = listProperty.propertyPath;

            // Configure elements
            titleLabel.text = listTitle;

            addButton.clicked += () => ShowAddItemMenu(type,listProperty, serializedObject, RebuildList);

               // Function to rebuild the list UI
            void RebuildList()
            {
                // Re-obtain fresh property reference from serializedObject to avoid stale references
                serializedObject.Update();
                var currentArrayProperty = serializedObject.FindProperty(propertyPath);

                // Get current number of UI elements (excluding empty message)
                var currentUICount = contentContainer.childCount;

                // Check for empty message in header container instead of content container
                var headerContainer = listContainer.Q<VisualElement>("list-header");
                var emptyMessageLabel = headerContainer.Children().FirstOrDefault(child =>
                    child is Label label && label.ClassListContains("octoputs-custom-list-empty-message")) as Label;
                var hasEmptyMessage = emptyMessageLabel != null;

                if (hasEmptyMessage)
                {
                    currentUICount = 0; // Empty message doesn't count as list item
                }

                var targetCount = currentArrayProperty.arraySize;

                // Remove empty message if we now have items
                if (hasEmptyMessage && targetCount > 0)
                {
                    // Remove the empty message from header container
                    headerContainer.Remove(emptyMessageLabel);
                    contentContainer.Clear();
                    currentUICount = 0;
                }

                // Add empty message if we have no items - add it to header instead of content
                if (targetCount == 0 && !hasEmptyMessage)
                {
                    contentContainer.Clear();

                    var emptyLabel = new Label(emptyListMessage);
                    emptyLabel.AddToClassList("octoputs-custom-list-empty-message");

                    // Ensure the header container aligns all children to center
                    headerContainer.AddToClassList("octoputs-custom-list-header-centered");

                    headerContainer.Insert(headerContainer.IndexOf(addButton), emptyLabel);

                    return;
                }

                // Remove excess UI elements if array shrunk
                while (currentUICount > targetCount)
                {
                    contentContainer.RemoveAt(contentContainer.childCount - 1);
                    currentUICount--;
                }

                // Add missing UI elements if array grew
                for (int i = currentUICount; i < targetCount; i++)
                {
                    CreateListItem(contentContainer, currentArrayProperty, serializedObject, i);
                }
            }
            
            // Apply stylesheet
            AddJungleStyleSheet(listContainer);

            // Initial build
            RebuildList();

            // Listen for array size changes to rebuild
            var tracker = new SerializedPropertyChangeTracker(listProperty);
            tracker.OnPropertyChanged += RebuildList;

            // Store both the rebuild callback and tracker in the list container's user data for cleanup
            listContainer.userData = new { RebuildCallback = (Action)RebuildList, Tracker = tracker };

            // Add cleanup when the container is removed from hierarchy
            listContainer.RegisterCallback<DetachFromPanelEvent>(evt =>
            {
                if (listContainer.userData is { } userData)
                {
                    var userDataType = userData.GetType();
                    var trackerProperty = userDataType.GetProperty("Tracker");
                    if (trackerProperty?.GetValue(userData) is SerializedPropertyChangeTracker trackerToDispose)
                    {
                        trackerToDispose.Dispose();
                    }
                }
            });

            container.Add(listContainer);
            return listContainer;

         
        }
        

        private static void CreateListItem(
            VisualElement container,
            SerializedProperty arrayProperty,
            SerializedObject serializedObject,
            int index)
        {
            var template = Resources.Load<VisualTreeAsset>("CustomGenericListItem");
            if (template == null)
            {
                Debug.LogError("Could not load CustomGenericListItem.uxml from Resources folder");
                return;
            }

           
            
            // Clone the template
            var itemContainer = template.CloneTree();

            var contentContainer = itemContainer.Q<VisualElement>("item-content");

            var element = arrayProperty.GetArrayElementAtIndex(index);
            
            object managedRef = null;
            string displayName = "Unknown Type";
            bool isUnityObject = false;

            // Check if this is actually a managed reference property
            if (element.propertyType == SerializedPropertyType.ManagedReference)
            {
                try
                {
                    managedRef = element.managedReferenceValue;

                    if (managedRef == null)
                    {
                        RemoveListItem(arrayProperty, serializedObject, index);
                        return;
                    }

                    // Check if this managed reference is actually a Unity Object
                    if (managedRef is UnityEngine.Object unityObj)
                    {
                        isUnityObject = true;
                        displayName = EditorUtils.FormatTypeName(unityObj.GetType());
                    }
                    else
                    {
                        displayName = EditorUtils.FormatTypeName(managedRef.GetType());
                    }
                    
                }
                catch (System.InvalidOperationException ex)
                {
                    Debug.LogWarning($"Failed to access managedReferenceValue: {ex.Message}");
                    // Fall back to treating as regular property
                    displayName = element.displayName;
                    if (string.IsNullOrEmpty(displayName))
                    {
                        displayName = $"Item {index}";
                    }
                }
            }
            else
            {
                // Handle non-managed reference properties
                if (element.propertyType == SerializedPropertyType.ObjectReference)
                {
                    var objRef = element.objectReferenceValue;
                    if (objRef == null)
                    {
                        RemoveListItem(arrayProperty, serializedObject, index);
                        return;
                    }
                    displayName = EditorUtils.FormatTypeName(objRef.GetType());
                    managedRef = objRef;
                    isUnityObject = true;
                }
                else
                {
                    // For other property types, use the property name or a generic display name
                    displayName = element.displayName;
                    if (string.IsNullOrEmpty(displayName))
                    {
                        displayName = $"Item {index}";
                    }
                }
            }

            // Hide the entire item header since we'll use the foldout header for the title
            var itemHeader = itemContainer.Q<VisualElement>("item-header");
            if (itemHeader != null)
            {
                itemHeader.style.display = DisplayStyle.None;
            }

            var buttonContainer = ConfigureButtons(container, arrayProperty, serializedObject, index, itemContainer);
            var foldout = new Foldout();
            foldout.text = displayName; // Keep class name in foldout header
            foldout.AddToClassList("octoputs-custom-list-title-foldout");

            // Access the foldout's toggle (header) to add buttons to it
            var foldoutToggle = foldout.Q<Toggle>();
            if (foldoutToggle != null)
            {
                foldoutToggle.style.flexDirection = FlexDirection.Row;
                foldoutToggle.style.justifyContent = Justify.SpaceBetween;
                foldoutToggle.style.alignItems = Align.Center;

                var label = foldoutToggle.Q<Label>();
                if (label != null)
                {
                    label.style.flexGrow = 1;
                    label.AddToClassList("octoputs-custom-list-title-label");
                }

                foldoutToggle.Add(buttonContainer);
            }

            // Try to find and use a custom property drawer for this type
            PropertyDrawer customDrawer = null;
            bool useCustomDrawer = false;
            VisualElement customDrawerElement = null;

            if (managedRef != null)
            {
                customDrawer = TryCreateCustomPropertyDrawer(managedRef.GetType());
                if (customDrawer != null)
                {
                    customDrawerElement = customDrawer.CreatePropertyGUI(element);
                    if (customDrawerElement != null)
                    {
                        useCustomDrawer = true;
                    }
                }
            }

            if (useCustomDrawer)
            {
                customDrawerElement.Bind(serializedObject);
                foldout.Add(customDrawerElement);

                contentContainer.Add(foldout);
            }
            else
            {
         
                if (isUnityObject && managedRef is UnityEngine.Object unityObject)
                {

                    var tempSerializedObject = new SerializedObject(unityObject);
                    var iterator = tempSerializedObject.GetIterator();
                    
                    if (iterator.NextVisible(true))
                    {
                        do
                        {
                            if (iterator.propertyPath == "m_Script")
                                continue;

                            var propertyField = new PropertyField(iterator.Copy());
                            propertyField.AddToClassList("octoputs-custom-list-property-field");

                            propertyField.Bind(tempSerializedObject);
                            foldout.Add(propertyField);
                        }
                        while (iterator.NextVisible(false));
                    }

                    // If no properties were added, show the element as a simple property field
                    if (foldout.childCount == 0)
                    {
                        var propertyField = new PropertyField(element);
                        propertyField.AddToClassList("octoputs-custom-list-property-field");
                        propertyField.label = "";
                        propertyField.Bind(serializedObject);
                        foldout.Add(propertyField);
                    }
                }
                else
                {
                    // Add child properties to the foldout for non-Unity objects
                    var childProperty = element.Copy();
                    var endProperty = childProperty.GetEndProperty();

                    if (childProperty.NextVisible(true)) // Enter the first child
                    {
                        do
                        {
                            if (SerializedProperty.EqualContents(childProperty, endProperty))
                                break;

                            var propertyField = new PropertyField(childProperty.Copy());
                            propertyField.AddToClassList("octoputs-custom-list-property-field");
                            propertyField.Bind(serializedObject);
                            foldout.Add(propertyField);
                        }
                        while (childProperty.NextVisible(false));
                    }

                    // If no child properties, add a simple property field
                    if (foldout.childCount == 0)
                    {
                        var propertyField = new PropertyField(element);
                        propertyField.AddToClassList("octoputs-custom-list-property-field");
                        propertyField.label = "";
                        propertyField.Bind(serializedObject);
                        foldout.Add(propertyField);
                    }
                }

                contentContainer.Add(foldout);
            }

            container.Add(itemContainer);
        }

        private static VisualElement ConfigureButtons(VisualElement container, SerializedProperty arrayProperty,
            SerializedObject serializedObject, int index, TemplateContainer itemContainer)
        {
            // Get references to elements
            var moveUpButton = itemContainer.Q<Button>("move-up-button");
            var moveDownButton = itemContainer.Q<Button>("move-down-button");
            var removeButton = itemContainer.Q<Button>("remove-button");

            // Setup button functionality
            removeButton.clicked += () => RemoveListItem(arrayProperty, serializedObject, index);

            // Setup move up/down buttons - use the original index directly
            moveUpButton.clicked += () =>
            {
                if (index > 0)
                {
                    serializedObject.Update();
                    arrayProperty.MoveArrayElement(index, index - 1);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(serializedObject.targetObject);

                    // Force complete rebuild by clearing and recreating all items
                    container.Clear();
                    for (int i = 0; i < arrayProperty.arraySize; i++)
                    {
                        CreateListItem(container, arrayProperty, serializedObject, i);
                    }
                }
            };
            moveDownButton.clicked += () =>
            {
                if (index < arrayProperty.arraySize - 1)
                {
                    serializedObject.Update();
                    arrayProperty.MoveArrayElement(index, index + 1);
                    serializedObject.ApplyModifiedProperties();
                    EditorUtility.SetDirty(serializedObject.targetObject);

                    // Force complete rebuild by clearing and recreating all items
                    container.Clear();
                    for (int i = 0; i < arrayProperty.arraySize; i++)
                    {
                        CreateListItem(container, arrayProperty, serializedObject, i);
                    }
                }
            };

            // Enable/disable buttons based on position
            moveUpButton.SetEnabled(index > 0);
            moveDownButton.SetEnabled(index < arrayProperty.arraySize - 1);

            // Style the buttons
            moveUpButton.tooltip = "Move up";
            moveDownButton.tooltip = "Move down";

            var buttonContainer = new VisualElement();
            buttonContainer.style.flexDirection = FlexDirection.Row;
            buttonContainer.style.flexShrink = 0;

            buttonContainer.Add(moveUpButton);
            buttonContainer.Add(moveDownButton);
            buttonContainer.Add(removeButton);
            return buttonContainer;
        }


        private static void ShowAddItemMenu(Type type,SerializedProperty arrayProperty, SerializedObject serializedObject,
            Action rebuildCallback = null) 
        {
            EditorUtils.ShowAddTypeMenu(type,
                itemType => AddItemDataToList(arrayProperty, serializedObject, itemType, rebuildCallback), null,
                $"Add {type.Name}");
        }

        
        private static void ShowAddItemMenu<T>(SerializedProperty arrayProperty, SerializedObject serializedObject,
            Action rebuildCallback = null) where T : class
        {
            EditorUtils.ShowAddTypeMenu<T>(
                itemType => AddItemDataToList(arrayProperty, serializedObject, itemType, rebuildCallback), null,
                $"Add {typeof(T).Name}");
        }

        private static void AddItemDataToList(SerializedProperty arrayProperty, SerializedObject serializedObject,
            Type itemType, Action rebuildCallback = null)
        {
            serializedObject.Update();
            SerializedProperty newElement;

            if (typeof(Component).IsAssignableFrom(itemType))
            {
                GameObject targetGameObject = ((Component)serializedObject.targetObject).gameObject;

                Undo.RecordObject(serializedObject.targetObject, $"Add {itemType.Name}");
                Undo.RecordObject(targetGameObject, $"Add {itemType.Name} Component");


                var newComponent = targetGameObject.AddComponent(itemType);

                EditorUtility.SetDirty(targetGameObject);

                serializedObject.Update();


                var updatedArrayProperty = serializedObject.FindProperty(arrayProperty.propertyPath);

                updatedArrayProperty.arraySize++;
                newElement = updatedArrayProperty.GetArrayElementAtIndex(updatedArrayProperty.arraySize - 1);

                newElement.objectReferenceValue = newComponent;

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(serializedObject.targetObject);

                rebuildCallback?.Invoke();
            }
            else
            {
                // For all other types, create an instance and add as managed reference
                var newItem = Activator.CreateInstance(itemType);

                // Add to the list
                arrayProperty.arraySize++;
                newElement = arrayProperty.GetArrayElementAtIndex(arrayProperty.arraySize - 1);

                // Set the managed reference value
                newElement.managedReferenceValue = newItem;
                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(serializedObject.targetObject);
                rebuildCallback?.Invoke();
            }
        }

        private static void RemoveListItem(SerializedProperty arrayProperty, SerializedObject serializedObject,
            int index)
        {
            if (index >= 0 && index < arrayProperty.arraySize)
            {
                serializedObject.Update();

                arrayProperty.DeleteArrayElementAtIndex(index);

                serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(serializedObject.targetObject);
            }
        }

        private static void AddJungleStyleSheet(VisualElement element)
        {
            // Check if the list style sheet is already loaded
            var listStyleSheet = Resources.Load<StyleSheet>("JungleListStyles");
            if (listStyleSheet != null)
            {
                // Only add if not already present
                if (!element.styleSheets.Contains(listStyleSheet))
                {
                    element.styleSheets.Add(listStyleSheet);
                }
            }
            else
            {
                Debug.LogWarning("Could not load JungleListStyles.uss from Resources folder");
            }
        }
    }

    /// <summary>
    /// Helper class to track changes to SerializedProperty arrays
    /// </summary>
    public class SerializedPropertyChangeTracker
    {
        private SerializedProperty trackedProperty;
        private int lastArraySize;
        public event Action OnPropertyChanged;

        public SerializedPropertyChangeTracker(SerializedProperty property)
        {
            trackedProperty = property;
            lastArraySize = property.arraySize;

            // Register for update callbacks
            EditorApplication.update += CheckForChanges;
        }

        private void CheckForChanges()
        {
            if (trackedProperty == null ||
                trackedProperty.serializedObject == null)
            {
                Dispose();
                return;
            }

            try
            {
                // Use Unity's ReferenceEquals for proper null checking of destroyed Unity objects
                if (ReferenceEquals(trackedProperty.serializedObject.targetObject, null))
                {
                    Dispose();
                    return;
                }

                trackedProperty.serializedObject.Update();
                var currentArraySize = trackedProperty.arraySize;
                if (currentArraySize != lastArraySize)
                {
                    lastArraySize = currentArraySize;
                    OnPropertyChanged?.Invoke();
                }
            }
            catch (System.NullReferenceException)
            {
                // Handle case where SerializedObject or its targetObject is in an invalid state
                Dispose();
            }
        }

        ~SerializedPropertyChangeTracker()
        {
            // Ensure we clean up the event subscription
            if (EditorApplication.update != null)
            {
                EditorApplication.update -= CheckForChanges;
            }
        }

        public void Dispose()
        {
            EditorApplication.update -= CheckForChanges;

            trackedProperty = null;
            OnPropertyChanged = null;
        }
    }

    /// <summary>
    /// Dummy class used for creating property drawer field info
    /// </summary>
    internal class DummyDrawerClass
    {
        [SerializeField] private object dummyField;
    }
}
#endif