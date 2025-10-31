#if UNITY_EDITOR
using System;
using Jungle.Actions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    /// <summary>
    /// Draws the inspector UI for individual SequenceAction steps so their timing and actions can be configured.
    /// </summary>
    [CustomPropertyDrawer(typeof(SequenceAction.Step))]
    public class ActionSequenceStepDrawer : PropertyDrawer
    {
        /// <summary>
        /// Builds the property field UI for the drawer.
        /// </summary>
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            root.AddToClassList("action-sequence-step");

            var foldout = new Foldout { value = false };
            foldout.text = BuildFoldoutLabel(property);
            root.Add(foldout);

            var actionProperty = RequireRelativeProperty(property, nameof(SequenceAction.Step.Action));
            var loopModeProperty = RequireRelativeProperty(property, nameof(SequenceAction.Step.loopMode));
            var loopCountProperty = RequireRelativeProperty(property, nameof(SequenceAction.Step.loopCount));
            var blockingProperty = RequireRelativeProperty(property, nameof(SequenceAction.Step.blocking));
            var startDelayProperty = RequireRelativeProperty(property, nameof(SequenceAction.Step.startDelay));
            var timeLimitedProperty = RequireRelativeProperty(property, nameof(SequenceAction.Step.timeLimited));
            var finishOnTimeoutProperty = RequireRelativeProperty(property, nameof(SequenceAction.Step.finishExecutionOnEndTime));
            var timeLimitProperty = RequireRelativeProperty(property, nameof(SequenceAction.Step.timeLimit));

            var actionField = new PropertyField(actionProperty);
            actionField.BindProperty(actionProperty);
            actionField.RegisterValueChangeCallback(_ => foldout.text = BuildFoldoutLabel(property));
            foldout.Add(actionField);

            // Add loop count display
            var loopInfoLabel = new Label();
            loopInfoLabel.style.fontSize = 11;
            loopInfoLabel.style.color = new StyleColor(new Color(0.7f, 0.7f, 0.7f));
            loopInfoLabel.style.marginTop = 2;
            loopInfoLabel.style.marginBottom = 4;
            foldout.Add(loopInfoLabel);

            var loopCountContainer = new VisualElement();

            var loopModeField = new PropertyField(loopModeProperty);
            loopModeField.BindProperty(loopModeProperty);
            // Update UI and foldout label immediately when loop mode changes
            loopModeField.RegisterValueChangeCallback(_ =>
            {
                UpdateLoopModeUI();
                foldout.text = BuildFoldoutLabel(property);
            });
            foldout.Add(loopModeField);

            loopCountContainer.style.marginLeft = 16f;
            var loopCountField = new PropertyField(loopCountProperty);
            loopCountField.BindProperty(loopCountProperty);
            // Update foldout label when loop count changes
            loopCountField.RegisterValueChangeCallback(_ => 
            {
                UpdateLoopInfoLabel();
                foldout.text = BuildFoldoutLabel(property);
            });
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

            void UpdateTimeLimitedVisibility()
            {
                SetElementDisplay(timeOptionsContainer, timeLimitedProperty.boolValue);
            }

            void UpdateLoopModeUI()
            {
                var loopMode = (SequenceAction.StepLoopMode)loopModeProperty.enumValueIndex;
                var showLoopCount = loopMode == SequenceAction.StepLoopMode.Limited;
                SetElementDisplay(loopCountContainer, showLoopCount);
                UpdateLoopInfoLabel();
            }

            void UpdateLoopInfoLabel()
            {
                var loopMode = (SequenceAction.StepLoopMode)loopModeProperty.enumValueIndex;
                switch (loopMode)
                {
                    case SequenceAction.StepLoopMode.Once:
                        loopInfoLabel.text = "Executes once";
                        break;
                    case SequenceAction.StepLoopMode.Infinite:
                        loopInfoLabel.text = "Loops infinitely (∞)";
                        break;
                    case SequenceAction.StepLoopMode.Limited:
                        var loopCount = Mathf.Max(1, loopCountProperty.intValue);
                        loopInfoLabel.text = $"Loops {loopCount} times";
                        break;
                }
            }

            UpdateTimeLimitedVisibility();
            UpdateLoopModeUI();

            root.TrackPropertyValue(timeLimitedProperty, _ => UpdateTimeLimitedVisibility());
            root.TrackPropertyValue(actionProperty, _ => foldout.text = BuildFoldoutLabel(property));
            // Refresh foldout text when loop mode or loop count change so loop info is always visible
            root.TrackPropertyValue(loopModeProperty, _ =>
            {
                UpdateLoopModeUI();
                foldout.text = BuildFoldoutLabel(property);
            });
            root.TrackPropertyValue(loopCountProperty, _ => 
            {
                UpdateLoopInfoLabel();
                foldout.text = BuildFoldoutLabel(property);
            });

            return root;
        }
        /// <summary>
        /// Builds the label displayed on the step foldout.
        /// </summary>
        private static string BuildFoldoutLabel(SerializedProperty property)
        {
            var actionProperty = property.FindPropertyRelative(nameof(SequenceAction.Step.Action));
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

            // Add loop information (if applicable) to the foldout label
            var loopInfo = string.Empty;
            var loopModeProp = property.FindPropertyRelative(nameof(SequenceAction.Step.loopMode));
            var loopCountProp = property.FindPropertyRelative(nameof(SequenceAction.Step.loopCount));
            if (loopModeProp != null)
            {
                var loopMode = (SequenceAction.StepLoopMode)loopModeProp.enumValueIndex;
                if (loopMode == SequenceAction.StepLoopMode.Infinite)
                {
                    loopInfo = " (Loops ∞)";
                }
                else if (loopMode == SequenceAction.StepLoopMode.Limited && loopCountProp != null)
                {
                    var loopCount = Mathf.Max(1, loopCountProp.intValue);
                    loopInfo = $" (Loops {loopCount}x)";
                }
            }

            return $"Step: {displayName}{loopInfo}";
        }
        /// <summary>
        /// Resolves the concrete managed-reference type for the property.
        /// </summary>
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
        /// <summary>
        /// Retrieves a required relative property and throws if missing.
        /// </summary>
        private static SerializedProperty RequireRelativeProperty(SerializedProperty parent, string name)
        {
            var property = parent.FindPropertyRelative(name);
            if (property == null)
            {
                throw new ArgumentException($"Property '{name}' was not found relative to '{parent.propertyPath}'.");
            }

            return property;
        }
        /// <summary>
        /// Applies display flags to the foldout entry.
        /// </summary>
        private static void SetElementDisplay(VisualElement element, bool visible)
        {
            element.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}
#endif
