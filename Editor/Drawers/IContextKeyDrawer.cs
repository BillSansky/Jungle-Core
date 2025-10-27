#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Jungle.Values.Context;
using UnityEngine;

namespace Jungle.Editor
{
    [CustomPropertyDrawer(typeof(IContextKey), true)]
    public class IContextKeyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            container.style.flexDirection = FlexDirection.Row;

            var label = new Label(property.displayName);
            label.style.minWidth = 120;
            label.style.unityTextAlign = TextAnchor.MiddleLeft;

            var targetObject = property.serializedObject.targetObject;
            var fieldValue = fieldInfo.GetValue(targetObject);

            if (fieldValue is IContextKey contextKey)
            {
                string contextName = contextKey.GetContextName(contextKey.ContextKey);

                var textField = new TextField();
                textField.value = contextName;
                textField.isReadOnly = true;
                textField.style.flexGrow = 1;

                container.Add(label);
                container.Add(textField);
            }
            else
            {
                var errorLabel = new Label("Not an IContextKey");
                errorLabel.style.color = UnityEngine.Color.red;

                container.Add(label);
                container.Add(errorLabel);
            }

            return container;
        }
    }
}
#endif
