#if UNITY_EDITOR
using System.Collections.Generic;
using Jungle.Actions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Jungle.Editor
{
    [CustomPropertyDrawer(typeof(ActionSequence))]
    public class ActionSequenceDrawer : PropertyDrawer
    {
        private const float LabelColumnWidth = 220f;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var root = new VisualElement();
            root.style.flexDirection = FlexDirection.Column;

            var foldout = new Foldout
            {
                text = property.displayName,
                value = property.isExpanded
            };

            foldout.RegisterValueChangedCallback(evt => property.isExpanded = evt.newValue);
            root.Add(foldout);

            var modeProp = property.FindPropertyRelative(nameof(ActionSequence.Mode));
            var sequenceTimeLimitProp = property.FindPropertyRelative(nameof(ActionSequence.SequenceTimeLimit));
            var finishOnLimitProp = property.FindPropertyRelative(nameof(ActionSequence.FinishOnSequenceTimeLimit));
            var stepsProp = property.FindPropertyRelative(nameof(ActionSequence.Steps));

            foldout.Add(new PropertyField(modeProp));
            foldout.Add(new PropertyField(sequenceTimeLimitProp));
            foldout.Add(new PropertyField(finishOnLimitProp));
            foldout.Add(new PropertyField(stepsProp));

            var timeline = new ActionSequenceTimeline(property);
            foldout.Add(timeline);

            root.Bind(property.serializedObject);
            return root;
        }

        private sealed class ActionSequenceTimeline : VisualElement
        {
            private readonly SerializedObject serializedObject;
            private readonly string propertyPath;

            private readonly Label headerLabel;
            private readonly VisualElement legendContainer;
            private readonly VisualElement rowsContainer;
            private readonly Label modeInfoLabel;

            public ActionSequenceTimeline(SerializedProperty property)
            {
                serializedObject = property.serializedObject;
                propertyPath = property.propertyPath;

                style.marginTop = 6f;
                style.paddingTop = 6f;
                style.paddingBottom = 8f;
                style.paddingLeft = 8f;
                style.paddingRight = 8f;
                style.borderTopLeftRadius = 4f;
                style.borderTopRightRadius = 4f;
                style.borderBottomLeftRadius = 4f;
                style.borderBottomRightRadius = 4f;
                style.borderBottomWidth = 1f;
                style.borderTopWidth = 1f;
                style.borderLeftWidth = 1f;
                style.borderRightWidth = 1f;
                style.borderBottomColor = EditorGUIUtility.isProSkin ? new Color(0.25f, 0.25f, 0.25f) : new Color(0.7f, 0.7f, 0.7f);
                style.borderTopColor = style.borderBottomColor;
                style.borderLeftColor = style.borderBottomColor;
                style.borderRightColor = style.borderBottomColor;
                style.backgroundColor = EditorGUIUtility.isProSkin
                    ? new Color(0.13f, 0.13f, 0.13f, 0.9f)
                    : new Color(0.92f, 0.92f, 0.92f, 0.9f);

                headerLabel = new Label("Timeline Preview");
                headerLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                headerLabel.style.marginBottom = 4f;

                legendContainer = new VisualElement();
                legendContainer.style.flexDirection = FlexDirection.Row;
                legendContainer.style.flexWrap = Wrap.Wrap;
                legendContainer.style.marginBottom = 6f;
                legendContainer.style.justifyContent = Justify.FlexStart;

                rowsContainer = new VisualElement();
                rowsContainer.style.flexDirection = FlexDirection.Column;
                rowsContainer.style.marginTop = 2f;

                modeInfoLabel = new Label();
                modeInfoLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
                modeInfoLabel.style.marginTop = 6f;
                modeInfoLabel.style.fontSize = 11;
                modeInfoLabel.style.color = EditorGUIUtility.isProSkin ? new Color(0.8f, 0.8f, 0.8f) : new Color(0.2f, 0.2f, 0.2f);

                Add(headerLabel);
                Add(legendContainer);
                Add(rowsContainer);
                Add(modeInfoLabel);

                CreateLegend();
                Refresh();

                var propertyCopy = property.Copy();
                this.TrackPropertyValue(propertyCopy, _ => Refresh());

                var stepsCopy = property.FindPropertyRelative(nameof(ActionSequence.Steps)).Copy();
                this.TrackPropertyValue(stepsCopy, _ => Refresh());

                var modeCopy = property.FindPropertyRelative(nameof(ActionSequence.Mode)).Copy();
                this.TrackPropertyValue(modeCopy, _ => Refresh());

                var sequenceLimitCopy = property.FindPropertyRelative(nameof(ActionSequence.SequenceTimeLimit)).Copy();
                this.TrackPropertyValue(sequenceLimitCopy, _ => Refresh());
            }

            private void Refresh()
            {
                serializedObject.UpdateIfRequiredOrScript();

                var rootProperty = serializedObject.FindProperty(propertyPath);
                var stepsProp = rootProperty.FindPropertyRelative(nameof(ActionSequence.Steps));
                var modeProp = rootProperty.FindPropertyRelative(nameof(ActionSequence.Mode));
                var sequenceTimeLimitProp = rootProperty.FindPropertyRelative(nameof(ActionSequence.SequenceTimeLimit));

                rowsContainer.Clear();

                if (stepsProp.arraySize == 0)
                {
                    var empty = new Label("No steps defined.");
                    empty.style.unityTextAlign = TextAnchor.MiddleLeft;
                    empty.style.color = EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.3f, 0.3f, 0.3f);
                    rowsContainer.Add(empty);
                }
                else
                {
                    var entries = BuildEntries(stepsProp);
                    var totalDuration = DetermineTimelineDuration(entries, modeProp, sequenceTimeLimitProp);
                    BuildRows(entries, totalDuration);
                }

                var mode = (ActionSequence.ProcessMode)modeProp.enumValueIndex;
                if (mode == ActionSequence.ProcessMode.Once)
                {
                    modeInfoLabel.text = string.Empty;
                }
                else if (mode == ActionSequence.ProcessMode.Loop)
                {
                    modeInfoLabel.text = "Sequence loops indefinitely.";
                }
                else
                {
                    var limit = sequenceTimeLimitProp.floatValue;
                    modeInfoLabel.text = limit > 0f
                        ? $"Sequence repeats until T+{limit:0.##}s." : "Sequence repeats while time remains.";
                }
            }

            private void CreateLegend()
            {
                legendContainer.Clear();

                legendContainer.Add(CreateLegendEntry(new Color(0.26f, 0.55f, 0.95f, 0.9f), "Blocking"));
                legendContainer.Add(CreateLegendEntry(new Color(0.3f, 0.8f, 0.45f, 0.9f), "Parallel"));
                legendContainer.Add(CreateLegendEntry(new Color(0.95f, 0.67f, 0.25f, 0.9f), "Loops"));
                legendContainer.Add(CreateLegendEntry(new Color(0.4f, 0.4f, 0.4f, 0.9f), "Unknown duration"));
            }

            private static VisualElement CreateLegendEntry(Color color, string label)
            {
                var entry = new VisualElement();
                entry.style.flexDirection = FlexDirection.Row;
                entry.style.alignItems = Align.Center;

                var swatch = new VisualElement();
                swatch.style.width = 12f;
                swatch.style.height = 12f;
                swatch.style.backgroundColor = color;
                swatch.style.borderBottomLeftRadius = 2f;
                swatch.style.borderBottomRightRadius = 2f;
                swatch.style.borderTopLeftRadius = 2f;
                swatch.style.borderTopRightRadius = 2f;
                swatch.style.marginRight = 4f;

                var labelElement = new Label(label);
                labelElement.style.fontSize = 11;
                labelElement.style.color = EditorGUIUtility.isProSkin ? new Color(0.8f, 0.8f, 0.8f) : new Color(0.2f, 0.2f, 0.2f);

                entry.style.marginRight = 8f;
                entry.style.marginBottom = 4f;

                entry.Add(swatch);
                entry.Add(labelElement);
                return entry;
            }

            private void BuildRows(IReadOnlyList<TimelineEntry> entries, float totalDuration)
            {
                var fallbackWidthFraction = entries.Count > 0 ? 1f / entries.Count : 1f;
                var fallbackStartFractions = CalculateFallbackStartFractions(entries, totalDuration, fallbackWidthFraction);

                for (var i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];

                    var row = new VisualElement();
                    row.style.flexDirection = FlexDirection.Row;
                    row.style.alignItems = Align.Stretch;
                    row.style.marginBottom = 4f;

                    var labelColumn = new VisualElement();
                    labelColumn.style.width = LabelColumnWidth;
                    labelColumn.style.flexShrink = 0f;
                    labelColumn.style.flexDirection = FlexDirection.Column;

                    var title = new Label($"{i + 1}. {entry.DisplayName}");
                    title.style.unityFontStyleAndWeight = FontStyle.Bold;
                    title.style.fontSize = 12;

                    var info = new Label(BuildInfoLabel(entry));
                    info.style.fontSize = 11;
                    info.style.color = EditorGUIUtility.isProSkin ? new Color(0.75f, 0.75f, 0.75f) : new Color(0.25f, 0.25f, 0.25f);

                    labelColumn.Add(title);
                    labelColumn.Add(info);

                    var barColumn = new VisualElement();
                    barColumn.style.flexGrow = 1f;
                    barColumn.style.height = 36f;
                    barColumn.style.position = Position.Relative;
                    barColumn.style.backgroundColor = EditorGUIUtility.isProSkin ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.88f, 0.88f, 0.88f);
                    barColumn.style.borderBottomLeftRadius = 3f;
                    barColumn.style.borderBottomRightRadius = 3f;
                    barColumn.style.borderTopLeftRadius = 3f;
                    barColumn.style.borderTopRightRadius = 3f;
                    barColumn.style.overflow = Overflow.Hidden;

                    var bar = CreateBar(entry, totalDuration, fallbackStartFractions[i], fallbackWidthFraction);
                    barColumn.Add(bar);

                    row.Add(labelColumn);
                    row.Add(barColumn);
                    rowsContainer.Add(row);
                }
            }

            private static float[] CalculateFallbackStartFractions(IReadOnlyList<TimelineEntry> entries, float totalDuration, float fallbackWidthFraction)
            {
                var result = new float[entries.Count];
                var hasLastKnownStart = false;
                var lastKnownStart = 0f;

                for (var i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];

                    if (entry.StartTime.HasValue)
                    {
                        var fraction = Mathf.Clamp01(totalDuration > 0f ? entry.StartTime.Value / totalDuration : 0f);
                        result[i] = fraction;
                        lastKnownStart = fraction;
                        hasLastKnownStart = true;
                    }
                    else if (entry.Blocking)
                    {
                        var fraction = Mathf.Clamp01(i * fallbackWidthFraction);
                        result[i] = fraction;
                        lastKnownStart = fraction;
                        hasLastKnownStart = true;
                    }
                    else
                    {
                        result[i] = hasLastKnownStart ? lastKnownStart : 0f;
                    }
                }

                return result;
            }

            private static VisualElement CreateBar(TimelineEntry entry, float totalDuration, float fallbackStartFraction, float fallbackWidthFraction)
            {
                var startFraction = entry.StartTime.HasValue
                    ? Mathf.Clamp01(entry.StartTime.Value / totalDuration)
                    : Mathf.Clamp01(fallbackStartFraction);

                var widthFraction = entry.Duration.HasValue
                    ? Mathf.Clamp01(entry.Duration.Value / totalDuration)
                    : Mathf.Clamp01(fallbackWidthFraction * 0.6f);

                var baseColor = entry.Blocking
                    ? new Color(0.26f, 0.55f, 0.95f, 0.9f)
                    : new Color(0.3f, 0.8f, 0.45f, 0.9f);

                if (!entry.Duration.HasValue)
                {
                    baseColor = Color.Lerp(baseColor, new Color(0.35f, 0.35f, 0.35f, 0.9f), 0.35f);
                }

                var bar = new VisualElement();
                bar.style.position = Position.Absolute;
                bar.style.left = new Length(startFraction * 100f, LengthUnit.Percent);
                bar.style.width = new Length(widthFraction * 100f, LengthUnit.Percent);
                bar.style.top = 6f;
                bar.style.bottom = 6f;
                bar.style.minWidth = 8f;
                bar.style.backgroundColor = baseColor;
                bar.style.borderBottomLeftRadius = 3f;
                bar.style.borderBottomRightRadius = 3f;
                bar.style.borderTopLeftRadius = 3f;
                bar.style.borderTopRightRadius = 3f;

                if (entry.LoopTillEnd)
                {
                    var loopStripe = new VisualElement
                    {
                        style =
                        {
                            position = Position.Absolute,
                            left = 2f,
                            right = 2f,
                            top = 0f,
                            height = 2f,
                            backgroundColor = new Color(1f, 1f, 1f, 0.75f)
                        }
                    };
                    bar.Add(loopStripe);
                }

                var label = new Label(BuildTimingLabel(entry));
                label.style.unityTextAlign = TextAnchor.MiddleCenter;
                label.style.fontSize = 11;
                label.style.color = Color.white;
                label.style.unityFontStyleAndWeight = FontStyle.Bold;
                label.style.whiteSpace = WhiteSpace.Normal;

                bar.Add(label);
                return bar;
            }

            private static string BuildInfoLabel(TimelineEntry entry)
            {
                var info = entry.Blocking ? "Blocking" : "Parallel";
                if (entry.LoopTillEnd)
                {
                    info += " • Loops";
                }

                info += entry.TimeLimited ? " • Time-limited" : " • Untimed";
                return info;
            }
        }

        private static List<TimelineEntry> BuildEntries(SerializedProperty stepsProp)
        {
            var entries = new List<TimelineEntry>(stepsProp.arraySize);
            var currentTime = 0f;
            var timeKnown = true;

            for (var i = 0; i < stepsProp.arraySize; i++)
            {
                var stepProp = stepsProp.GetArrayElementAtIndex(i);
                var blocking = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.blocking)).boolValue;
                var loopTillEnd = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.loopTillEnd)).boolValue;
                var timeLimited = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.timeLimited)).boolValue;
                var timeLimit = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.timeLimit)).floatValue;
                var actionProp = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.Action));

                float? startTime = timeKnown ? currentTime : (float?)null;
                float? duration = timeLimited && timeLimit > 0f ? timeLimit : (float?)null;

                entries.Add(new TimelineEntry
                {
                    DisplayName = BuildStepDisplayName(actionProp, i),
                    Blocking = blocking,
                    LoopTillEnd = loopTillEnd,
                    TimeLimited = timeLimited,
                    StartTime = startTime,
                    Duration = duration
                });

                if (blocking)
                {
                    if (timeKnown && duration.HasValue)
                    {
                        currentTime += duration.Value;
                    }
                    else
                    {
                        timeKnown = false;
                    }
                }
            }

            return entries;
        }

        private static float DetermineTimelineDuration(List<TimelineEntry> entries, SerializedProperty modeProp, SerializedProperty sequenceTimeLimitProp)
        {
            var max = 0f;

            foreach (var entry in entries)
            {
                if (entry.StartTime.HasValue && entry.Duration.HasValue)
                {
                    max = Mathf.Max(max, entry.StartTime.Value + entry.Duration.Value);
                }
            }

            if ((ActionSequence.ProcessMode)modeProp.enumValueIndex == ActionSequence.ProcessMode.TimeLimited)
            {
                var sequenceLimit = sequenceTimeLimitProp.floatValue;
                if (sequenceLimit > 0f)
                {
                    max = Mathf.Max(max, sequenceLimit);
                }
            }

            if (max <= 0f)
            {
                max = Mathf.Max(entries.Count, 1);
            }

            return max;
        }

        private static string BuildStepDisplayName(SerializedProperty actionProp, int index)
        {
            if (string.IsNullOrEmpty(actionProp.managedReferenceFullTypename))
            {
                return "(Unassigned Action)";
            }

            var fullTypeName = actionProp.managedReferenceFullTypename;
            var lastDot = fullTypeName.LastIndexOf('.');
            var displayName = lastDot >= 0 && lastDot < fullTypeName.Length - 1
                ? fullTypeName.Substring(lastDot + 1)
                : fullTypeName;

            var assemblySeparator = displayName.LastIndexOf(' ');
            if (assemblySeparator >= 0)
            {
                displayName = displayName.Substring(assemblySeparator + 1);
            }

            displayName = displayName.Replace('+', '.');
            return ObjectNames.NicifyVariableName(displayName);
        }

        private static string BuildTimingLabel(TimelineEntry entry)
        {
            var startLabel = entry.StartTime.HasValue
                ? $"Start T+{entry.StartTime.Value:0.##}s"
                : "Start T+?";

            string durationLabel;
            if (entry.Duration.HasValue)
            {
                durationLabel = $"Runs {entry.Duration.Value:0.##}s";
            }
            else if (entry.Blocking)
            {
                durationLabel = "Duration ?";
            }
            else
            {
                durationLabel = "Runs alongside";
            }

            return $"{startLabel}\n{durationLabel}";
        }

        private sealed class TimelineEntry
        {
            public string DisplayName = string.Empty;
            public bool Blocking;
            public bool LoopTillEnd;
            public bool TimeLimited;
            public float? StartTime;
            public float? Duration;
        }
    }
}
#endif
