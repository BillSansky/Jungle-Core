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
            var overrideModeProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.overrideSequenceMode));
            var stepModeProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.mode));
            var loopProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.loopTillEnd));
            var loopCountProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.loopCount));
            var blockingProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.blocking));
            var startDelayProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.startDelay));
            var timeLimitedProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.timeLimited));
            var finishOnTimeoutProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.finishExecutionOnEndTime));
            var timeLimitProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.timeLimit));
            var modeTimeLimitProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.modeTimeLimit));
            var finishOnModeLimitProperty = RequireRelativeProperty(property, nameof(ActionSequence.Step.finishOnModeTimeLimit));

            var modeProp = property.serializedObject.FindProperty("Mode");

            var actionField = new PropertyField(actionProperty);
            actionField.BindProperty(actionProperty);
            actionField.RegisterValueChangeCallback(_ => foldout.text = BuildFoldoutLabel(property));
            foldout.Add(actionField);

            var overrideModeField = new PropertyField(overrideModeProperty);
            overrideModeField.BindProperty(overrideModeProperty);
            foldout.Add(overrideModeField);

            var stepModeField = new PropertyField(stepModeProperty);
            stepModeField.BindProperty(stepModeProperty);
            foldout.Add(stepModeField);

            var loopContainer = new VisualElement();
            var loopField = new PropertyField(loopProperty);
            loopField.BindProperty(loopProperty);
            loopContainer.Add(loopField);
            foldout.Add(loopContainer);

            var loopCountContainer = new VisualElement();
            loopCountContainer.style.marginLeft = 16f;
            var loopCountField = new PropertyField(loopCountProperty);
            loopCountField.BindProperty(loopCountProperty);
            loopCountContainer.Add(loopCountField);
            foldout.Add(loopCountContainer);

            var blockingField = new PropertyField(blockingProperty);
            blockingField.BindProperty(blockingProperty);
            foldout.Add(blockingField);

            var startDelayField = new PropertyField(startDelayProperty);
            startDelayField.BindProperty(startDelayProperty);
            foldout.Add(startDelayField);

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

            var modeTimeLimitContainer = new VisualElement();
            modeTimeLimitContainer.style.marginLeft = 16f;

            var modeTimeLimitField = new PropertyField(modeTimeLimitProperty);
            modeTimeLimitField.BindProperty(modeTimeLimitProperty);
            modeTimeLimitContainer.Add(modeTimeLimitField);

            var finishOnModeLimitField = new PropertyField(finishOnModeLimitProperty);
            finishOnModeLimitField.BindProperty(finishOnModeLimitProperty);
            modeTimeLimitContainer.Add(finishOnModeLimitField);

            foldout.Add(modeTimeLimitContainer);

            void UpdateTimeLimitedVisibility()
            {
                SetElementDisplay(timeOptionsContainer, timeLimitedProperty.boolValue);
            }

            void UpdateModeUI()
            {
                var overrideMode = overrideModeProperty.boolValue;
                var stepMode = (ActionSequence.ProcessMode)stepModeProperty.enumValueIndex;
                var sequenceMode = modeProp != null
                    ? (ActionSequence.ProcessMode)modeProp.enumValueIndex
                    : ActionSequence.ProcessMode.Once;

                var effectiveMode = overrideMode ? stepMode : sequenceMode;

                stepModeField.SetEnabled(overrideMode);

                var loopsRelevant = effectiveMode != ActionSequence.ProcessMode.Once;
                SetElementDisplay(loopContainer, !overrideMode && loopsRelevant);

                var showLoopCount = loopsRelevant;
                SetElementDisplay(loopCountContainer, showLoopCount);
                loopCountField.SetEnabled(loopsRelevant && (overrideMode || loopProperty.boolValue));

                var showModeTimeLimit = overrideMode && stepMode == ActionSequence.ProcessMode.TimeLimited;
                SetElementDisplay(modeTimeLimitContainer, showModeTimeLimit);
            }

            UpdateTimeLimitedVisibility();
            UpdateModeUI();

            root.TrackPropertyValue(timeLimitedProperty, _ => UpdateTimeLimitedVisibility());
            root.TrackPropertyValue(actionProperty, _ => foldout.text = BuildFoldoutLabel(property));
            root.TrackPropertyValue(overrideModeProperty, _ => UpdateModeUI());
            root.TrackPropertyValue(stepModeProperty, _ => UpdateModeUI());
            root.TrackPropertyValue(loopProperty, _ => UpdateModeUI());

            if (modeProp != null)
            {
                root.TrackPropertyValue(modeProp, _ => UpdateModeUI());
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
