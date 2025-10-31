#if UNITY_EDITOR
using System.Collections;
using Jungle.Attributes;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    /// <summary>
    /// Shows a searchable dropdown that lets designers pick a type matching the JungleClassSelectionAttribute filter.
    /// </summary>
    [CustomPropertyDrawer(typeof(JungleClassSelectionAttribute))]
    public class JungleClassSelectionAttributeDrawer : PropertyDrawer
    {
        /// <summary>
        /// Builds the property field UI for the drawer.
        /// </summary>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var classSelectionAttribute = (JungleClassSelectionAttribute)attribute;
            var baseType = EditorUtils.ResolveElementType(
                classSelectionAttribute?.BaseType,
                fieldInfo?.FieldType,
                property.serializedObject.targetObject.GetType(),
                property.propertyPath
            );
            ;

            if (baseType == null) return new Label("Invalid base type");

            var root = new VisualElement(); // wrapper to isolate our changes
            var field = new PropertyField(property);

            root.Add(field);
            //field.BindProperty(property);

            var supportsObject =  typeof(Object).IsAssignableFrom(baseType);
            var supportsManagedReference = property.propertyType == SerializedPropertyType.ManagedReference;

            if (supportsObject || supportsManagedReference)
            {
                // Guard against multiple runs
                bool initialized = false;

                // Defer mutations until after the attach/layout pass
                field.schedule.Execute(() =>
                {
                    if (initialized || field.panel == null) return;
                    initialized = true;


                    EditorUtils.SetupFieldWithClassSelectionButton(field, baseType, property, fieldInfo);
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