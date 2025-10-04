#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    [CustomPropertyDrawer(typeof(SRA))]
    public class SerializedReferenceAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
       
            var baseType = EditorUtils.ResolveElementType(
                null,
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