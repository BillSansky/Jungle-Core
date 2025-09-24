#if UNITY_EDITOR
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using Jungle.Attributes;
using Jungle.Conditions;
using Jungle.Editor;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor.Conditions
{
    internal abstract class ConditionDrawerBase : PropertyDrawer
    {
        private const string BaseTemplatePath = "ConditionDrawer";
        private const string EditorStyleSheetPath = "JungleEditorStyles";
        private const string ListStyleSheetPath = "JungleListStyles";

        protected abstract string ContentTemplatePath { get; }

        protected virtual void InitializeContent(VisualElement contentRoot, SerializedProperty property)
        {
        }

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var baseTemplate = Resources.Load<VisualTreeAsset>(BaseTemplatePath);
            if (baseTemplate == null)
            {
                Debug.LogError($"Could not load {BaseTemplatePath}.uxml from Resources folder");
                return new Label("Missing Condition Template");
            }

            var root = baseTemplate.CloneTree();

            AddStyleSheet(root, EditorStyleSheetPath);
            AddStyleSheet(root, ListStyleSheetPath);

            var contentContainer = root.Q<VisualElement>("condition-content") ?? root;
            VisualElement contentRoot = contentContainer;

            if (!string.IsNullOrEmpty(ContentTemplatePath))
            {
                var contentTemplate = Resources.Load<VisualTreeAsset>(ContentTemplatePath);
                if (contentTemplate == null)
                {
                    Debug.LogError($"Could not load {ContentTemplatePath}.uxml from Resources folder");
                }
                else
                {
                    contentRoot = contentTemplate.CloneTree();
                    contentContainer.Add(contentRoot);
                }
            }

            SetupConditionMetadata(root, property);

            InitializeContent(contentRoot, property);

            root.BindProperty(property);

            ProcessJungleListAttributes(root, property);

            return root;
        }

        private void SetupConditionMetadata(VisualElement root, SerializedProperty property)
        {
            var conditionInstance = GetConditionInstance(property);
            var conditionType = conditionInstance?.GetType() ?? fieldInfo?.FieldType;

            var titleLabel = root.Q<Label>("condition-title");
            if (titleLabel != null && conditionType != null)
            {
                titleLabel.text = EditorUtils.FormatTypeName(conditionType);
            }

            var infoContainer = root.Q<VisualElement>("condition-info");
            var descriptionLabel = root.Q<Label>("condition-description");

            if (conditionType != null)
            {
                var infoAttribute = conditionType.GetCustomAttribute<JungleInfoAttribute>();
                if (!string.IsNullOrEmpty(infoAttribute?.Description))
                {
                    if (descriptionLabel != null)
                    {
                        descriptionLabel.text = infoAttribute.Description;
                    }
                    if (infoContainer != null)
                    {
                        infoContainer.style.display = DisplayStyle.Flex;
                    }
                    return;
                }
            }

            if (infoContainer != null)
            {
                infoContainer.style.display = DisplayStyle.None;
            }
        }

        protected object GetConditionInstance(SerializedProperty property)
        {
            if (property == null)
            {
                return null;
            }

            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                return property.managedReferenceValue;
            }

            if (fieldInfo != null && property.serializedObject?.targetObject != null)
            {
                try
                {
                    return fieldInfo.GetValue(property.serializedObject.targetObject);
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }

            return null;
        }

        private void ProcessJungleListAttributes(VisualElement root, SerializedProperty property)
        {
            if (root == null || property == null)
            {
                return;
            }

            var conditionInstance = GetConditionInstance(property);
            var conditionType = conditionInstance?.GetType() ?? fieldInfo?.FieldType;
            if (conditionType == null)
            {
                return;
            }

            var fields = conditionType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (var field in fields)
            {
                var listAttribute = field.GetCustomAttribute<JungleListAttribute>();
                if (listAttribute == null)
                {
                    continue;
                }

                var listProperty = property.FindPropertyRelative(field.Name);
                if (listProperty == null)
                {
                    continue;
                }

                var containerName = !string.IsNullOrEmpty(listAttribute.UIElementName)
                    ? listAttribute.UIElementName
                    : GenerateUIElementName(field.Name);

                var containerElement = root.Q<VisualElement>(containerName);
                if (containerElement == null)
                {
                    Debug.LogWarning(
                        $"Could not find UI element '{containerName}' for field '{field.Name}' in {conditionType.Name}");
                    continue;
                }

                var elementType = GetListElementType(field.FieldType);
                if (elementType == null)
                {
                    Debug.LogWarning($"Could not determine list element type for field '{field.Name}'");
                    continue;
                }

                CustomGenericListDrawer.CreateCustomList(
                    elementType,
                    containerElement,
                    listProperty,
                    property.serializedObject,
                    listAttribute.ListTitle,
                    listAttribute.EmptyMessage);
            }
        }

        private static void AddStyleSheet(VisualElement element, string styleSheetPath)
        {
            if (element == null || string.IsNullOrEmpty(styleSheetPath))
            {
                return;
            }

            var styleSheet = Resources.Load<StyleSheet>(styleSheetPath);
            if (styleSheet == null)
            {
                Debug.LogWarning($"Could not load {styleSheetPath}.uss from Resources folder");
                return;
            }

            if (!element.styleSheets.Contains(styleSheet))
            {
                element.styleSheets.Add(styleSheet);
            }
        }

        private static Type GetListElementType(Type fieldType)
        {
            if (fieldType == null)
            {
                return null;
            }

            if (fieldType.IsArray)
            {
                return fieldType.GetElementType();
            }

            if (fieldType.IsGenericType && fieldType.GetGenericTypeDefinition() == typeof(System.Collections.Generic.List<>))
            {
                return fieldType.GetGenericArguments().FirstOrDefault();
            }

            return null;
        }

        private static string GenerateUIElementName(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                return string.Empty;
            }

            var builder = new StringBuilder(fieldName.Length * 2);
            for (int i = 0; i < fieldName.Length; i++)
            {
                var character = fieldName[i];
                if (i > 0 && char.IsUpper(character))
                {
                    builder.Append('-');
                }
                builder.Append(char.ToLowerInvariant(character));
            }

            builder.Append("-content");
            return builder.ToString();
        }
    }
}
#endif
