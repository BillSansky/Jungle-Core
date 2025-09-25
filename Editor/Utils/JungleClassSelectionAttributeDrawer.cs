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

            var root = new VisualElement();                  // wrapper to isolate our changes
            var field = new PropertyField(property);
            root.Add(field);
            field.BindProperty(property);

            var supportsComponents = baseType != null && typeof(Component).IsAssignableFrom(baseType);
            var supportsManagedReference = property.propertyType == SerializedPropertyType.ManagedReference;

            if (baseType != null && (supportsComponents || supportsManagedReference))
            {
                // Guard against multiple runs
                bool initialized = false;

                // Defer mutations until after the attach/layout pass
                field.schedule.Execute(() =>
                {
                    if (initialized || field.panel == null) return;
                    initialized = true;

                    // Make sure Setup... only adds children under 'field' (or 'root'),
                    // not by reparenting 'field' itself.
                    EditorUtils.SetupFieldWithClassSelectionButton(field, baseType, property);
                });
            }
            else
            {
                Debug.LogWarning(
                    $"JungleClassSelectionAttribute requires a valid base type but '{baseType?.Name ?? fieldInfo?.FieldType.Name}' is not supported."
                );
            }

            return root;
        }

    }
}
#endif
