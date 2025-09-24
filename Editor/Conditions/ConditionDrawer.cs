#if UNITY_EDITOR
using System.Reflection;
using Jungle.Attributes;
using Jungle.Conditions;
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

                var propertyField = new PropertyField(fieldProperty.Copy());
                propertyField.AddToClassList("jungle-marginfield");
                propertyField.BindProperty(fieldProperty);
                contentRoot.Add(propertyField);
            }
        }

    }
}
#endif
