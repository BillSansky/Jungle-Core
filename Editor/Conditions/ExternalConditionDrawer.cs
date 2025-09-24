#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Jungle.Conditions;

namespace Octoputs.Editor.Conditions
{
    [CustomPropertyDrawer(typeof(ExternalCondition))]
    public class ExternalConditionDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            // Load the UXML template
            var template = Resources.Load<VisualTreeAsset>("ExternalConditionDrawer");
            if (template == null)
            {
                Debug.LogError("Could not load ExternalConditionDrawer.uxml from Resources folder");
                return new Label("Missing UXML Template");
            }

            var root = template.CloneTree();

            // Apply Octoputs stylesheet
            var styleSheet = Resources.Load<StyleSheet>("OctoputsListStyles");
            if (styleSheet != null)
            {
                root.styleSheets.Add(styleSheet);
            }

            // Get UI elements
            var conditionProviderField = root.Q<ObjectField>("condition-provider-field");
            var errorContainer = root.Q<VisualElement>("error-container");
            var errorLabel = root.Q<Label>("error-label");
            

            // Find the ConditionProvider property
            var conditionProviderProp = property.FindPropertyRelative("conditionProvider");
            if (conditionProviderProp == null)
            {
                return new Label("ConditionProvider property not found");
            }

            // Configure ObjectField settings (binding is handled by UXML)
            conditionProviderField.objectType = typeof(Object);
            conditionProviderField.allowSceneObjects = true;

            // Validation function
            void ValidateConditionProvider()
            {
                var conditionProvider = conditionProviderProp.objectReferenceValue;

                if (conditionProvider == null)
                {
                    // Hide error when no object is assigned
                    errorContainer.style.display = DisplayStyle.None;
                    return;
                }

                // Check if the object implements IBooleanCondition
                if (conditionProvider is IBooleanCondition)
                {
                    // Valid - hide error
                    errorContainer.style.display = DisplayStyle.None;
                }
                else
                {
                    // Invalid - show error and null out the reference
                    errorLabel.text = $"'{conditionProvider.GetType().Name}' does not implement IBooleanCondition interface";
                    errorContainer.style.display = DisplayStyle.Flex;

                    // Clear the invalid reference
                    conditionProviderProp.objectReferenceValue = null;
                    conditionProviderProp.serializedObject.ApplyModifiedProperties();
                }
            }

            // Initial validation
            ValidateConditionProvider();

            // Listen for changes to the ConditionProvider field
            conditionProviderField.RegisterValueChangedCallback(evt =>
            {
                ValidateConditionProvider();
            });

            return root;
        }
    }
}
#endif
