#if UNITY_EDITOR
using Jungle.Conditions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using EditorUtils = Jungle.Editor.EditorUtils;

namespace Jungle.Editor.Conditions
{
    [CustomPropertyDrawer(typeof(ExternalCondition))]
    public class ExternalConditionDrawer : ConditionDrawerBase
    {
        private const string TemplatePath = "ExternalConditionDrawer";

        protected override string ContentTemplatePath => TemplatePath;

        protected override void InitializeContent(VisualElement contentRoot, SerializedProperty property)
        {
            var conditionProviderField = contentRoot.Q<ObjectField>("condition-provider-field");
            var errorContainer = contentRoot.Q<VisualElement>("error-container");
            var errorLabel = contentRoot.Q<Label>("error-label");
            var hasErrorElements = errorContainer != null && errorLabel != null;

            if (!hasErrorElements)
            {
                Debug.LogWarning("Missing error UI elements for ExternalCondition drawer");
            }

            var conditionProviderProp = property.FindPropertyRelative("conditionProvider");
            if (conditionProviderProp == null)
            {
                Debug.LogError("ConditionProvider property not found on ExternalCondition");
                return;
            }

            if (conditionProviderField != null)
            {
                conditionProviderField.objectType = typeof(Object);
                conditionProviderField.allowSceneObjects = true;

                EditorUtils.BindPropertySafely(conditionProviderField, conditionProviderProp);

                conditionProviderField.RegisterValueChangedCallback(_ => ValidateConditionProvider());
            }

            ValidateConditionProvider();

            void ValidateConditionProvider()
            {
                if (!hasErrorElements)
                {
                    return;
                }

                var conditionProvider = conditionProviderProp.objectReferenceValue;

                if (conditionProvider == null)
                {
                    errorContainer.style.display = DisplayStyle.None;
                    return;
                }

                if (conditionProvider is IBooleanCondition)
                {
                    errorContainer.style.display = DisplayStyle.None;
                }
                else
                {
                    errorLabel.text =
                        $"'{conditionProvider.GetType().Name}' does not implement IBooleanCondition interface";
                    errorContainer.style.display = DisplayStyle.Flex;

                    conditionProviderProp.objectReferenceValue = null;
                    conditionProviderProp.serializedObject.ApplyModifiedProperties();
                }
            }
        }
    }
}
#endif
