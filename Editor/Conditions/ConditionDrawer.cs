#if UNITY_EDITOR
using System.Reflection;
using Jungle.Attributes;
using Jungle.Conditions;
using EditorUtils = Jungle.Editor.EditorUtils;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor.Conditions
{
    [CustomPropertyDrawer(typeof(Condition), true)]
    public class ConditionDrawer : ConditionDrawerBase
    {
        protected override string ContentTemplatePath => null;

        protected override void InitializeContent(VisualElement contentRoot, SerializedProperty property)
        {
            if (contentRoot == null)
            {
                Debug.LogError("Missing content root for Condition drawer");
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
                if (field.Name == "negateCondition")
                {
                    continue;
                }

                var hasSerializeFieldAttribute = field.GetCustomAttribute<SerializeField>() != null;
                var hasSerializeReferenceAttribute = field.GetCustomAttribute<SerializeReference>() != null;
                var isSerializedField = field.IsPublic || hasSerializeFieldAttribute || hasSerializeReferenceAttribute;

                if (!isSerializedField)
                {
                    continue;
                }

                if (field.GetCustomAttribute<JungleListAttribute>() != null)
                {
                    continue;
                }

                var fieldProperty = property.FindPropertyRelative(field.Name);
                if (fieldProperty == null)
                {
                    continue;
                }

                var fieldPropertyCopy = fieldProperty.Copy();
                var propertyField = new PropertyField(fieldPropertyCopy);
                propertyField.AddToClassList("jungle-marginfield");
                EditorUtils.BindPropertySafely(propertyField, fieldProperty);
                AttachClassSelectionButtonIfNeeded(propertyField, field, fieldProperty);
                contentRoot.Add(propertyField);
            }
        }

        private static void AttachClassSelectionButtonIfNeeded(PropertyField propertyField, FieldInfo field,
            SerializedProperty fieldProperty)
        {
            var classSelectionAttribute = field.GetCustomAttribute<JungleClassSelectionAttribute>();
            if (classSelectionAttribute == null)
            {
                return;
            }

            var baseType = classSelectionAttribute.BaseType ?? field.FieldType;
            if (baseType == null)
            {
                var fieldTypeName = field.FieldType != null ? field.FieldType.Name : "Unknown";
                var message = "JungleClassSelectionAttribute requires a valid base type but '" + fieldTypeName + "' is not supported.";
                Debug.LogWarning(message);
                return;
            }

            var propertyForSelection = fieldProperty.Copy();
            var isInitialized = false;
            propertyField.RegisterCallback<AttachToPanelEvent>(_ =>
            {
                if (isInitialized)
                {
                    return;
                }

                isInitialized = true;
                EditorUtils.SetupFieldWithClassSelectionButton(propertyField, baseType, propertyForSelection);
            });
        }

    }
}
#endif
