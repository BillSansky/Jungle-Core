﻿#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using Jungle.Attributes;

namespace Jungle.Editor
{
    public class ClassSelectionPopup : EditorWindow
    {
        private List<Type> classTypes;
        private Action<Type> onClassSelected;
        private const float WindowWidth = 400f;
        private const float Padding = 12f;

        private ScrollView scrollView;
        private VisualElement contentContainer;

        public static void Show(Vector2 position, List<Type> types, Action<Type> onTypeSelected, string title = "Select Class")
        {
            var window = CreateInstance<ClassSelectionPopup>();
            window.classTypes = types ?? new List<Type>();
            window.onClassSelected = onTypeSelected;
            window.titleContent = new GUIContent(title);

            // Calculate window size based on categorized content
            float totalHeight = 60f; // Title and padding
            var categorizedTypes = window.GroupTypesByCategory(window.classTypes);
            
            foreach (var category in categorizedTypes)
            {
                totalHeight += 30f; // Category header height
                foreach (var classType in category.Value)
                {
                    var classInfo = window.GetClassInfo(classType);
                    totalHeight += window.CalculateItemHeight(classInfo.description) + 8f;
                }
                totalHeight += 10f; // Category spacing
            }

            float maxHeight = Screen.height * 0.6f;
            totalHeight = Mathf.Min(totalHeight, maxHeight);

            // Position the popup to appear connected to the button
            Vector2 adjustedPosition = AdjustPositionForButton(position, WindowWidth, totalHeight);
            var rect = new Rect(adjustedPosition.x, adjustedPosition.y, WindowWidth, totalHeight);
            window.position = rect;
            window.ShowPopup();
            window.Focus();
        }

        private static Vector2 AdjustPositionForButton(Vector2 buttonPosition, float windowWidth, float windowHeight)
        {
            // The position is already in screen coordinates, so we just need to offset it properly
            Vector2 adjustedPosition = buttonPosition;
            
            // Add some offset to appear below the button instead of directly on it
            adjustedPosition.y += 25f; // Appear below the button
            
            // Ensure the popup doesn't go off-screen
            float screenWidth = Screen.width;
            float screenHeight = Screen.height;
            
            // Check if popup would go off the right edge of screen
            if (adjustedPosition.x + windowWidth > screenWidth)
            {
                adjustedPosition.x = screenWidth - windowWidth - 10f; // 10px margin from edge
            }
            
            // Check if popup would go off the bottom edge of screen
            if (adjustedPosition.y + windowHeight > screenHeight)
            {
                // Position above the button instead
                adjustedPosition.y = buttonPosition.y - windowHeight - 5f;
                
                // If still off-screen, position at top with margin
                if (adjustedPosition.y < 0)
                {
                    adjustedPosition.y = 10f;
                }
            }
            
            // Ensure minimum margin from left edge
            if (adjustedPosition.x < 10f)
            {
                adjustedPosition.x = 10f;
            }
            
            return adjustedPosition;
        }

        private void CreateGUI()
        {
            if (classTypes == null)
            {
                classTypes = new List<Type>();
            }

            // Load the UXML file
            var visualTree = Resources.Load<VisualTreeAsset>("ClassSelectionPopup");
            if (visualTree != null)
            {
                visualTree.CloneTree(rootVisualElement);
            }
            else
            {
                Debug.LogError("Could not load ActionSelectionPopup.uxml from Resources folder");
                return;
            }

            // Load the stylesheet
            var styleSheet = Resources.Load<StyleSheet>("OctoputsClassSelectionStyles");
            if (styleSheet != null)
            {
                rootVisualElement.styleSheets.Add(styleSheet);
            }

            // Get references to UI elements
            scrollView = rootVisualElement.Q<ScrollView>("scrollView");
            contentContainer = scrollView?.contentContainer ?? rootVisualElement.Q<VisualElement>("contentContainer");

            if (contentContainer == null)
            {
                Debug.LogError("Could not find contentContainer in UXML");
                return;
            }

            // Group types by category and add them to the UI
            var categorizedTypes = GroupTypesByCategory(classTypes);
            foreach (var category in categorizedTypes.OrderBy(kvp => kvp.Key))
            {
                CreateCategoryHeader(category.Key);
                foreach (var classType in category.Value.OrderBy(t => t.Name))
                {
                    CreateClassItem(classType);
                }
                
                // Add spacing between categories (except for the last one)
                if (category.Key != categorizedTypes.Keys.Last())
                {
                    CreateCategorySpacing();
                }
            }
           

            // Handle escape key to close popup
            rootVisualElement.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.keyCode == KeyCode.Escape)
                {
                    Close();
                    evt.StopPropagation();
                }
            });

            rootVisualElement.focusable = true;
            rootVisualElement.Focus();
        }

        private void OnLostFocus()
        {
            // Safely close the popup when focus is lost
            // Check if the window is still valid before closing
            if (this != null && this)
            {
                Close();
            }
        }

        private void OnDestroy()
        {
            // Clean up any remaining references
            onClassSelected = null;
            classTypes = null;
        }

        private void CreateClassItem(Type classType)
        {
            var displayName = EditorUtils.FormatActionTypeName(classType);
            var classInfo = GetClassInfo(classType);

            // Load the class item template
            var template = Resources.Load<VisualTreeAsset>("ClassItemTemplate");
            if (template == null)
            {
                Debug.LogError("Could not load ClassItemTemplate.uxml from Resources folder");
                return;
            }

            // Clone the template
            var itemElement = template.CloneTree();

            // Get references to elements
            var itemContainer = itemElement.Q<VisualElement>("itemContainer");
            var iconContainer = itemElement.Q<VisualElement>("iconContainer");
            var titleLabel = itemElement.Q<Label>("titleLabel");
            var descriptionLabel = itemElement.Q<Label>("descriptionLabel");

            if (itemContainer == null || iconContainer == null || titleLabel == null || descriptionLabel == null)
            {
                Debug.LogError("Could not find required elements in ClassItemTemplate.uxml");
                return;
            }

            // Set minimum height
            itemContainer.style.minHeight = CalculateItemHeight(classInfo.description);

            // Create and add icon
            var icon = CreateClassIcon(classType, classInfo);
            iconContainer.Add(icon);

            // Set title text
            titleLabel.text = displayName;

            // Set description text and visibility
            if (!string.IsNullOrEmpty(classInfo.description))
            {
                descriptionLabel.text = classInfo.description;
                descriptionLabel.style.display = DisplayStyle.Flex;
            }
            else
            {
                descriptionLabel.style.display = DisplayStyle.None;
            }

            // Add click handling
            itemContainer.RegisterCallback<MouseDownEvent>(evt =>
            {
                if (evt.button == 0) // Left mouse button
                {
                    onClassSelected?.Invoke(classType);
                    Close();
                    evt.StopPropagation();
                }
            });

            // Add the entire item element (including separator) to content container
            contentContainer.Add(itemElement);
        }

        private VisualElement CreateClassIcon(Type classType, (string description, string iconPath, string category) classInfo)
        {
            Texture2D icon = null;

            // Try to load custom icon first
            if (!string.IsNullOrEmpty(classInfo.iconPath))
            {
                icon = Resources.Load<Texture2D>(classInfo.iconPath);
                if (icon == null)
                {
                    try
                    {
                        var iconContent = EditorGUIUtility.IconContent(classInfo.iconPath);
                        if (iconContent != null)
                        {
                            icon = iconContent.image as Texture2D;
                        }
                    }
                    catch
                    {
                        // Icon doesn't exist, will fall back to default
                    }
                }
            }

            // Fallback to default icon based on class type
            if (icon == null)
            {
                string defaultIcon = GetDefaultIconForClassType(classType);
                try
                {
                    var iconContent = EditorGUIUtility.IconContent(defaultIcon);
                    if (iconContent != null)
                    {
                        icon = iconContent.image as Texture2D;
                    }
                }
                catch
                {
                    // Default icon doesn't exist either, will use colored fallback
                }
            }

            if (icon != null)
            {
                var image = new Image();
                image.image = icon;
                image.AddToClassList("class-item-icon-image");
                image.scaleMode = ScaleMode.ScaleToFit;
                return image;
            }
            else
            {
                // Create colored fallback with letter
                var fallbackContainer = new VisualElement();
                fallbackContainer.AddToClassList("class-item-icon-fallback");
                fallbackContainer.AddToClassList(GetIconCssClassForClassType(classType));

                var letterLabel = new Label(classType.Name.Substring(0, 1));
                letterLabel.AddToClassList("class-item-icon-letter");
                fallbackContainer.Add(letterLabel);

                return fallbackContainer;
            }
        }

        private float CalculateItemHeight(string description)
        {
            float baseHeight = 60f; // Minimum height for icon + padding

            if (!string.IsNullOrEmpty(description))
            {
                // Rough estimation: 15 pixels per line, assume ~50 characters per line
                int estimatedLines = Mathf.CeilToInt(description.Length / 50f);
                float descriptionHeight = estimatedLines * 15f;

                // Add title height (20px) + spacing
                float requiredHeight = 30f + descriptionHeight + Padding * 2;
                return Mathf.Max(baseHeight, requiredHeight);
            }

            return baseHeight;
        }

        private (string description, string iconPath, string category) GetClassInfo(Type classType)
        {
            if (classType.GetCustomAttributes(typeof(JungleInfoAttribute), true)
                    .FirstOrDefault() is JungleInfoAttribute attribute)
            {
                return (attribute.Description, attribute.IconPath, attribute.Category);
            }

            // Generate default description and category based on type name
            var defaultDescription = $"Generic type: {EditorUtils.FormatActionTypeName(classType)}";
            var defaultCategory = GetDefaultCategoryForClassType(classType);
            return (defaultDescription, null, defaultCategory);
        }

        private string GetDefaultCategoryForClassType(Type classType)
        {
            var typeName = classType.Name.ToLower();

            if (typeName.Contains("transform") || typeName.Contains("position") || typeName.Contains("rotation") || typeName.Contains("scale"))
                return "Transform";
            if (typeName.Contains("parent") || typeName.Contains("hierarchy"))
                return "Hierarchy";
            if (typeName.Contains("activation") || typeName.Contains("gameobject") || typeName.Contains("enable") || typeName.Contains("disable"))
                return "GameObject";
            if (typeName.Contains("audio") || typeName.Contains("sound"))
                return "Audio";
            if (typeName.Contains("physics") || typeName.Contains("rigidbody") || typeName.Contains("collider"))
                return "Physics";
            if (typeName.Contains("renderer") || typeName.Contains("material") || typeName.Contains("visual"))
                return "Rendering";
            if (typeName.Contains("layer"))
                return "Layers";
            if (typeName.Contains("animation") || typeName.Contains("animator"))
                return "Animation";

            return "General";
        }

        private string GetDefaultIconForClassType(Type classType)
        {
            var typeName = classType.Name.ToLower();

            if (typeName.Contains("scale")) return "ScaleTool";
            if (typeName.Contains("parent")) return "UnityEditor.HierarchyWindow";
            if (typeName.Contains("layer")) return "ViewToolOrbit";
            if (typeName.Contains("activation") || typeName.Contains("gameobject")) return "GameObject Icon";
            if (typeName.Contains("audio")) return "AudioSource Icon";
            if (typeName.Contains("physics")) return "Rigidbody Icon";
            if (typeName.Contains("transform")) return "Transform Icon";
            if (typeName.Contains("renderer") || typeName.Contains("material")) return "Material Icon";

            return "cs Script Icon"; // Default fallback
        }

        private string GetIconCssClassForClassType(Type classType)
        {
            var typeName = classType.Name.ToLower();

            if (typeName.Contains("scale")) return "class-icon-scale";
            if (typeName.Contains("parent")) return "class-icon-parent";
            if (typeName.Contains("layer")) return "class-icon-layer";
            if (typeName.Contains("activation")) return "class-icon-activation";
            if (typeName.Contains("audio")) return "class-icon-audio";
            if (typeName.Contains("physics")) return "class-icon-physics";

            return "class-icon-default";
        }
        
        private Dictionary<string, List<Type>> GroupTypesByCategory(List<Type> types)
        {
            var categorizedTypes = new Dictionary<string, List<Type>>();

            foreach (var type in types)
            {
                var classInfo = GetClassInfo(type);
                var category = classInfo.category;

                if (!categorizedTypes.ContainsKey(category))
                {
                    categorizedTypes[category] = new List<Type>();
                }

                categorizedTypes[category].Add(type);
            }

            return categorizedTypes;
        }

        private void CreateCategoryHeader(string categoryName)
        {
            var categoryHeader = new Label(categoryName);
            categoryHeader.AddToClassList("category-header");
            categoryHeader.style.fontSize = 14;
            categoryHeader.style.unityFontStyleAndWeight = FontStyle.Bold;
            categoryHeader.style.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            categoryHeader.style.marginTop = 8f;
            categoryHeader.style.marginBottom = 4f;
            categoryHeader.style.marginLeft = 8f;
            
            contentContainer.Add(categoryHeader);
        }

        private void CreateCategorySpacing()
        {
            var spacer = new VisualElement();
            spacer.style.height = 10f;
            contentContainer.Add(spacer);
        }
    }
}
#endif
