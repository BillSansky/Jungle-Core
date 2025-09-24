#if UNITY_EDITOR
using Jungle.Conditions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor.Conditions
{
    [CustomPropertyDrawer(typeof(CombinatoryCondition))]
    public class CombinatoryConditionDrawer : ConditionDrawerBase
    {
        private const string TemplatePath = "CombinatoryConditionDrawer";

        protected override string ContentTemplatePath => TemplatePath;

        protected override void InitializeContent(VisualElement contentRoot, SerializedProperty property)
        {
            if (contentRoot == null)
            {
                Debug.LogError("Missing content root for CombinatoryCondition drawer");
                return;
            }

            var logicalOperatorField = contentRoot.Q<PropertyField>("logical-operator-field");
            var logicalOperatorProperty = property.FindPropertyRelative("logicalOperator");

            if (logicalOperatorProperty == null)
            {
                Debug.LogError("logicalOperator property not found on CombinatoryCondition");
                return;
            }

            logicalOperatorField?.BindProperty(logicalOperatorProperty);
        }
    }
}
#endif
