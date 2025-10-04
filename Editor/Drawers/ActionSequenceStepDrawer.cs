#if UNITY_EDITOR
using System;
using Jungle.Actions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    [CustomPropertyDrawer(typeof(ActionSequence.Step))]
    public class ActionSequenceStepDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            root.AddToClassList("action-sequence-step");

            var foldout = new Foldout { value = false };
            foldout.text = BuildFoldoutLabel(property);
            root.Add(foldout);

            var actionProperty = RequireRelativeProperty(property, "Action");
            var loopProperty = RequireRelativeProperty(property, "loopTillEnd");
            var blockingProperty = RequireRelativeProperty(property, "blocking");
            var timeLimitedProperty = RequireRelativeProperty(property, "timeLimited");
            var finishOnTimeoutProperty = RequireRelativeProperty(property, "finishExecutionOnEndTime");
            var timeLimitProperty = RequireRelativeProperty(property, "timeLimit");

            var actionField = new PropertyField(actionProperty);
            actionField.BindProperty(actionProperty);
            actionField.RegisterValueChangeCallback(_ => foldout.text = BuildFoldoutLabel(property));
            foldout.Add(actionField);

            var loopContainer = new VisualElement();
            var loopField = new PropertyField(loopProperty);
            loopField.BindProperty(loopProperty);
            loopContainer.Add(loopField);
            foldout.Add(loopContainer);

            var blockingField = new PropertyField(blockingProperty);
            blockingField.BindProperty(blockingProperty);
            foldout.Add(blockingField);

            var timeLimitedField = new PropertyField(timeLimitedProperty);
            timeLimitedField.BindProperty(timeLimitedProperty);
            foldout.Add(timeLimitedField);

            var timeOptionsContainer = new VisualElement();
            timeOptionsContainer.style.marginLeft = 16f;

            var timeLimitField = new PropertyField(timeLimitProperty);
            timeLimitField.BindProperty(timeLimitProperty);
            timeOptionsContainer.Add(timeLimitField);

            var finishOnTimeoutField = new PropertyField(finishOnTimeoutProperty);
            finishOnTimeoutField.BindProperty(finishOnTimeoutProperty);
            timeOptionsContainer.Add(finishOnTimeoutField);

            foldout.Add(timeOptionsContainer);

            void UpdateTimeLimitedVisibility()
            {
                SetElementDisplay(timeOptionsContainer, timeLimitedProperty.boolValue);
            }

            void UpdateLoopVisibility()
            {
                var modeProperty = property.serializedObject.FindProperty("Mode");
                if (modeProperty == null)
                {
                    SetElementDisplay(loopContainer, true);
                    return;
                }

                var modeValue = (ActionSequence.ProcessMode)modeProperty.enumValueIndex;
                var isRelevant = modeValue != ActionSequence.ProcessMode.Once;
                SetElementDisplay(loopContainer, isRelevant);
            }

            UpdateTimeLimitedVisibility();
            UpdateLoopVisibility();

            root.TrackPropertyValue(timeLimitedProperty, _ => UpdateTimeLimitedVisibility());
            root.TrackPropertyValue(actionProperty, _ => foldout.text = BuildFoldoutLabel(property));

            var modeProp = property.serializedObject.FindProperty("Mode");
            if (modeProp != null)
            {
                root.TrackPropertyValue(modeProp, _ => UpdateLoopVisibility());
            }

            return root;
        }

        private static string BuildFoldoutLabel(SerializedProperty property)
        {
            var actionProperty = property.FindPropertyRelative("Action");
            if (actionProperty == null)
            {
                return "Step";
            }

            var type = ResolveManagedReferenceType(actionProperty);
            if (type == null)
            {
                return "Step (Empty)";
            }

            var displayName = EditorUtils.FormatActionTypeName(type);
            if (string.IsNullOrWhiteSpace(displayName))
            {
                displayName = type.Name;
            }

            return $"Step: {displayName}";
        }

        private static Type ResolveManagedReferenceType(SerializedProperty property)
        {
            var fullTypeName = property.managedReferenceFullTypename;
            if (string.IsNullOrEmpty(fullTypeName))
            {
                return null;
            }

            var segments = fullTypeName.Split(' ');
            if (segments.Length != 2)
            {
                return null;
            }

            var assemblyName = segments[0];
            var typeName = segments[1];
            var qualifiedName = $"{typeName}, {assemblyName}";
            return Type.GetType(qualifiedName);
        }

        private static SerializedProperty RequireRelativeProperty(SerializedProperty parent, string name)
        {
            var property = parent.FindPropertyRelative(name);
            if (property == null)
            {
                throw new ArgumentException($"Property '{name}' was not found relative to '{parent.propertyPath}'.");
            }

            return property;
        }

        private static void SetElementDisplay(VisualElement element, bool visible)
        {
            element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
#endif
