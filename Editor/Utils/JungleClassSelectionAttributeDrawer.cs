#if UNITY_EDITOR
using Jungle.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    [CustomPropertyDrawer(typeof(JungleClassSelectionAttribute))]
    public class JungleClassSelectionAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var classSelectionAttribute = (JungleClassSelectionAttribute)attribute;
            var baseType = classSelectionAttribute.BaseType ?? fieldInfo?.FieldType;

            var propertyField = new PropertyField(property);
            propertyField.BindProperty(property);

            var supportsComponents = baseType != null && typeof(Component).IsAssignableFrom(baseType);
            var supportsManagedReference = property.propertyType == SerializedPropertyType.ManagedReference;

            if (baseType != null && (supportsComponents || supportsManagedReference))
            {
                var isInitialized = false;
                propertyField.RegisterCallback<AttachToPanelEvent>(_ =>
                {
                    if (isInitialized)
                    {
                        return;
                    }

                    isInitialized = true;
                    EditorUtils.SetupFieldWithClassSelectionButton(propertyField, baseType, property);
                });
            }
            else
            {
                Debug.LogWarning(
                    $"JungleClassSelectionAttribute requires a valid base type but '{baseType?.Name ?? fieldInfo?.FieldType.Name}' is not supported."
                );
            }

            return propertyField;
        }
    }
}
#endif
