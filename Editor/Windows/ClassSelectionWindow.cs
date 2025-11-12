#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System;
using System.Collections.Generic;
using System.Linq;
using Jungle.Attributes;


namespace Jungle.Editor
{
    /// <summary>
    /// Popup window that presents grouped Jungle runtime types so designers can
    /// quickly pick an implementation for serialized references.
    /// </summary>
    public class ClassSelectionWindow : EditorWindow
    {
        private List<Type> classTypes;
        private Action<Type> onClassSelected;
        private const float WindowWidth = 400f;
        private const float Padding = 12f;

        private ScrollView scrollView;
        private VisualElement contentContainer;

        public static void Show(Vector2 position, List<Type> types, Action<Type> onTypeSelected, string title = "Select Class")
        {
            var window = CreateInstance<ClassSelectionWindow>();
            window.classTypes = types ?? new List<Type>();
            window.onClassSelected = onTypeSelected;
            window.titleContent = new GUIContent(title);

            // Calculate window size based on categorized content
            float totalHeight = 60f; // Title and padding
            var categoryTree = window.BuildCategoryTree(window.classTypes);
            totalHeight += window.CalculateTreeHeight(categoryTree, false);

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
            var visualTree = Resources.Load<VisualTreeAsset>("ClassSelectionWindow");
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

            // GetContext references to UI elements
            scrollView = rootVisualElement.Q<ScrollView>("scrollView");
            contentContainer = scrollView?.contentContainer ?? rootVisualElement.Q<VisualElement>("contentContainer");

            if (contentContainer == null)
            {
                Debug.LogError("Could not find contentContainer in UXML");
                return;
            }

            // Group types by category path and add them to the UI
            var categoryTree = BuildCategoryTree(classTypes);

            var rootTypes = categoryTree.Types
                .OrderBy(t => EditorUtils.FormatActionTypeName(t), StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var classType in rootTypes)
            {
                CreateClassItem(classType, 0);
            }

            if (rootTypes.Count > 0 && categoryTree.Children.Count > 0)
            {
                CreateCategorySpacing(0);
            }

            AddCategoryChildrenToUI(categoryTree, 0);
           

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

        private void CreateClassItem(Type classType, int indentLevel)
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

            // GetContext references to elements
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

            // Apply indentation
            var indentation = 16f + indentLevel * 14f;
            itemElement.style.marginLeft = indentation;

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
            if (classType.GetCustomAttributes(typeof(JungleClassInfoAttribute), true)
                    .FirstOrDefault() is JungleClassInfoAttribute attribute)
            {
                return (attribute.Description, attribute.IconPathOrKey, attribute.Category);
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
        
        private CategoryNode BuildCategoryTree(IEnumerable<Type> types)
        {
            var root = new CategoryNode(null);
            var categoryEntries = new List<(Type type, List<string> segments)>();

            foreach (var type in types)
            {
                var classInfo = GetClassInfo(type);
                var categoryPath = string.IsNullOrWhiteSpace(classInfo.category) ? "General" : classInfo.category;
                var segments = categoryPath
                    .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(segment => segment.Trim())
                    .Where(segment => !string.IsNullOrEmpty(segment))
                    .ToList();

                if (segments.Count == 0)
                {
                    segments.Add("General");
                }

                categoryEntries.Add((type, segments));
            }

            int sharedPrefixLength = GetSharedCategoryPrefixLength(categoryEntries.Select(entry => entry.segments).ToList());
            bool shouldTrimSharedPrefix = sharedPrefixLength > 0 &&
                                          categoryEntries.Any(entry => entry.segments.Count > sharedPrefixLength);

            foreach (var entry in categoryEntries)
            {
                var segments = shouldTrimSharedPrefix
                    ? entry.segments.Skip(sharedPrefixLength).Where(segment => !string.IsNullOrEmpty(segment)).ToList()
                    : entry.segments;

                if (segments.Count == 0)
                {
                    segments.Add("General");
                }

                var currentNode = root;
                foreach (var segment in segments)
                {
                    currentNode = currentNode.GetOrAddChild(segment);
                }

                currentNode.Types.Add(entry.type);
            }

            return root;
        }

        private static int GetSharedCategoryPrefixLength(IReadOnlyList<List<string>> categoryPaths)
        {
            if (categoryPaths == null || categoryPaths.Count == 0)
            {
                return 0;
            }

            int prefixLength = 0;
            while (true)
            {
                string candidate = null;

                for (int pathIndex = 0; pathIndex < categoryPaths.Count; pathIndex++)
                {
                    var path = categoryPaths[pathIndex];
                    if (path.Count <= prefixLength)
                    {
                        return prefixLength;
                    }

                    var segment = path[prefixLength];
                    if (candidate == null)
                    {
                        candidate = segment;
                        continue;
                    }

                    if (!string.Equals(candidate, segment, StringComparison.OrdinalIgnoreCase))
                    {
                        return prefixLength;
                    }
                }

                prefixLength++;
            }
        }

        private void CreateCategoryHeader(string categoryName, int depth)
        {
            var categoryHeader = new Label(categoryName);
            categoryHeader.AddToClassList("category-header");
            categoryHeader.style.fontSize = 14;
            categoryHeader.style.unityFontStyleAndWeight = FontStyle.Bold;
            categoryHeader.style.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            categoryHeader.style.marginTop = 8f;
            categoryHeader.style.marginBottom = 4f;
            categoryHeader.style.marginLeft = 8f + depth * 14f;

            contentContainer.Add(categoryHeader);
        }

        private void CreateCategorySpacing(int depth)
        {
            var spacer = new VisualElement();
            spacer.style.height = 10f;
            spacer.style.marginLeft = 8f + depth * 14f;
            contentContainer.Add(spacer);
        }

        private void AddCategoryChildrenToUI(CategoryNode parentNode, int depth)
        {
            var orderedChildren = parentNode.Children.Values
                .OrderBy(child => child.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            for (int index = 0; index < orderedChildren.Count; index++)
            {
                var child = orderedChildren[index];
                AddCategoryNodeToUI(child, depth);

                if (index < orderedChildren.Count - 1)
                {
                    CreateCategorySpacing(depth);
                }
            }
        }

        private void AddCategoryNodeToUI(CategoryNode node, int depth)
        {
            CreateCategoryHeader(node.Name, depth);

            var sortedTypes = node.Types
                .OrderBy(type => EditorUtils.FormatActionTypeName(type), StringComparer.OrdinalIgnoreCase)
                .ToList();

            foreach (var classType in sortedTypes)
            {
                CreateClassItem(classType, depth + 1);
            }

            AddCategoryChildrenToUI(node, depth + 1);
        }

        private float CalculateTreeHeight(CategoryNode node, bool includeHeader)
        {
            float height = 0f;

            if (includeHeader)
            {
                height += 30f;
            }

            foreach (var type in node.Types)
            {
                var classInfo = GetClassInfo(type);
                height += CalculateItemHeight(classInfo.description) + 8f;
            }

            var orderedChildren = node.Children.Values
                .OrderBy(child => child.Name, StringComparer.OrdinalIgnoreCase)
                .ToList();

            for (int index = 0; index < orderedChildren.Count; index++)
            {
                var child = orderedChildren[index];
                height += CalculateTreeHeight(child, true);

                if (index < orderedChildren.Count - 1)
                {
                    height += 10f;
                }
            }

            return height;
        }

        private sealed class CategoryNode
        {
            public string Name { get; }
            public Dictionary<string, CategoryNode> Children { get; }
            public List<Type> Types { get; }

            public CategoryNode(string name)
            {
                Name = name;
                Children = new Dictionary<string, CategoryNode>(StringComparer.OrdinalIgnoreCase);
                Types = new List<Type>();
            }

            public CategoryNode GetOrAddChild(string name)
            {
                if (!Children.TryGetValue(name, out var child))
                {
                    child = new CategoryNode(name);
                    Children[name] = child;
                }

                return child;
            }
        }
    }
}
#endif
