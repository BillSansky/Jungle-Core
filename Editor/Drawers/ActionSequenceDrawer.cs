#if UNITY_EDITOR
using System.Collections.Generic;
using Jungle.Actions;
using UnityEditor;
using UnityEngine;

namespace Jungle.Editor
{
    /// <summary>
    /// Custom drawer for <see cref="ActionSequence"/> that renders a timeline preview
    /// illustrating how each step is expected to execute.
    /// </summary>
    [CustomPropertyDrawer(typeof(ActionSequence))]
    public class ActionSequenceDrawer : PropertyDrawer
    {
        private const float HeaderHeight = 18f;
        private const float LegendHeight = 20f;
        private const float RowPadding = 4f;
        private const float RowHeight = 20f;
        private const float TimelineMargin = 6f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = EditorGUIUtility.singleLineHeight;

            if (!property.isExpanded)
            {
                return height;
            }

            height += EditorGUIUtility.standardVerticalSpacing;

            height += EditorGUIUtility.singleLineHeight * 3f;
            height += EditorGUIUtility.standardVerticalSpacing * 3f;

            var stepsProp = property.FindPropertyRelative(nameof(ActionSequence.Steps));
            height += EditorGUI.GetPropertyHeight(stepsProp, true);
            height += EditorGUIUtility.standardVerticalSpacing;

            height += CalculateTimelineHeight(stepsProp);

            return height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

            if (!property.isExpanded)
            {
                EditorGUI.EndProperty();
                return;
            }

            EditorGUI.indentLevel++;

            var contentY = foldoutRect.yMax + EditorGUIUtility.standardVerticalSpacing;
            var lineRect = new Rect(position.x, contentY, position.width, EditorGUIUtility.singleLineHeight);

            var modeProp = property.FindPropertyRelative(nameof(ActionSequence.Mode));
            var sequenceTimeLimitProp = property.FindPropertyRelative(nameof(ActionSequence.SequenceTimeLimit));
            var finishOnLimitProp = property.FindPropertyRelative(nameof(ActionSequence.FinishOnSequenceTimeLimit));
            var stepsProp = property.FindPropertyRelative(nameof(ActionSequence.Steps));

            EditorGUI.PropertyField(lineRect, modeProp);
            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PropertyField(lineRect, sequenceTimeLimitProp);
            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            EditorGUI.PropertyField(lineRect, finishOnLimitProp);
            lineRect.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

            var stepsHeight = EditorGUI.GetPropertyHeight(stepsProp, true);
            lineRect.height = stepsHeight;
            EditorGUI.PropertyField(lineRect, stepsProp, true);
            lineRect.y += stepsHeight + EditorGUIUtility.standardVerticalSpacing;

            var timelineHeight = CalculateTimelineHeight(stepsProp);
            lineRect.height = timelineHeight;
            DrawTimeline(lineRect, stepsProp, modeProp, sequenceTimeLimitProp);

            EditorGUI.indentLevel--;

            EditorGUI.EndProperty();
        }

        private static float CalculateTimelineHeight(SerializedProperty stepsProp)
        {
            var rowCount = Mathf.Max(stepsProp.arraySize, 1);
            return HeaderHeight + LegendHeight + (RowHeight + RowPadding) * rowCount + TimelineMargin * 2f;
        }

        private static void DrawTimeline(Rect rect, SerializedProperty stepsProp, SerializedProperty modeProp, SerializedProperty sequenceTimeLimitProp)
        {
            EditorGUI.DrawRect(rect, EditorGUIUtility.isProSkin
                ? new Color(0.13f, 0.13f, 0.13f, 0.9f)
                : new Color(0.9f, 0.9f, 0.9f, 0.9f));

            var headerRect = new Rect(rect.x + TimelineMargin, rect.y + TimelineMargin, rect.width - TimelineMargin * 2f, HeaderHeight);
            var rowsStartY = headerRect.yMax + LegendHeight + RowPadding;

            var title = new GUIContent("Timeline Preview", "Approximate start time and execution mode for each step.");
            EditorGUI.LabelField(headerRect, title, EditorStyles.boldLabel);

            if (stepsProp.arraySize == 0)
            {
                var emptyRect = new Rect(headerRect.x, rowsStartY, headerRect.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.LabelField(emptyRect, "No steps defined.", EditorStyles.centeredGreyMiniLabel);
                return;
            }

            var entries = BuildEntries(stepsProp);
            DrawLegend(headerRect);

            var totalDuration = DetermineTimelineDuration(entries, modeProp, sequenceTimeLimitProp);
            var rowsRect = new Rect(headerRect.x, rowsStartY, headerRect.width, entries.Count * (RowHeight + RowPadding));

            var labelWidth = Mathf.Min(220f, rowsRect.width * 0.45f);
            var barsRect = new Rect(rowsRect.x + labelWidth + RowPadding, rowsRect.y, rowsRect.width - labelWidth - RowPadding, rowsRect.height);

            Handles.color = new Color(0f, 0f, 0f, 0.3f);
            Handles.DrawLine(new Vector2(barsRect.x, rowsRect.y - RowPadding * 0.5f), new Vector2(barsRect.xMax, rowsRect.y - RowPadding * 0.5f));

            for (var i = 0; i < entries.Count; i++)
            {
                var entry = entries[i];
                var rowY = rowsRect.y + i * (RowHeight + RowPadding);

                var labelRect = new Rect(rowsRect.x, rowY, labelWidth, RowHeight);
                DrawStepLabel(labelRect, i, entry);

                var barRect = new Rect(barsRect.x, rowY + 2f, barsRect.width, RowHeight - 4f);
                DrawStepBar(barRect, entry, totalDuration, i, entries.Count);
            }

            var mode = (ActionSequence.ProcessMode)modeProp.enumValueIndex;
            if (mode != ActionSequence.ProcessMode.Once)
            {
                var infoRect = new Rect(headerRect.x, rect.yMax - TimelineMargin - EditorGUIUtility.singleLineHeight, headerRect.width, EditorGUIUtility.singleLineHeight);
                var info = mode == ActionSequence.ProcessMode.Loop
                    ? "Sequence loops indefinitely."
                    : "Sequence repeats while time remains.";
                EditorGUI.LabelField(infoRect, info, EditorStyles.miniLabel);
            }
        }

        private static void DrawLegend(Rect headerRect)
        {
            var legendY = headerRect.y + headerRect.height + 2f;
            var legendX = headerRect.x;
            var iconSize = new Vector2(12f, 12f);
            var style = EditorStyles.miniLabel;

            legendX = DrawLegendEntry(legendX, legendY, iconSize, style, new Color(0.26f, 0.55f, 0.95f, 0.9f), "Blocking");
            legendX = DrawLegendEntry(legendX, legendY, iconSize, style, new Color(0.3f, 0.8f, 0.45f, 0.9f), "Parallel");
            legendX = DrawLegendEntry(legendX, legendY, iconSize, style, new Color(0.95f, 0.67f, 0.25f, 0.9f), "Loops", true);
            DrawLegendEntry(legendX, legendY, iconSize, style, new Color(0.4f, 0.4f, 0.4f, 0.9f), "Unknown duration", false, true);
        }

        private static float DrawLegendEntry(float startX, float y, Vector2 size, GUIStyle style, Color color, string label, bool striped = false, bool dotted = false)
        {
            var rect = new Rect(startX, y, size.x, size.y);
            EditorGUI.DrawRect(rect, color);

            if (striped)
            {
                Handles.color = new Color(0.98f, 0.98f, 0.98f, 0.7f);
                var stripeY = rect.y + rect.height * 0.5f;
                Handles.DrawLine(new Vector2(rect.x, stripeY), new Vector2(rect.xMax, stripeY));
            }

            if (dotted)
            {
                Handles.color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
                var mid = rect.y + rect.height * 0.5f;
                Handles.DrawDottedLine(new Vector2(rect.x, mid), new Vector2(rect.xMax, mid), 2f);
            }

            var labelRect = new Rect(rect.xMax + 4f, y - (style.lineHeight - size.y) * 0.5f, 90f, style.lineHeight);
            EditorGUI.LabelField(labelRect, label, style);
            return labelRect.xMax + 12f;
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

                float? startTime = timeKnown ? currentTime : null;
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

        private static void DrawStepLabel(Rect rect, int index, TimelineEntry entry)
        {
            var title = $"{index + 1}. {entry.DisplayName}";
            EditorGUI.LabelField(rect, title, EditorStyles.label);

            var info = entry.Blocking ? "Blocking" : "Parallel";
            if (entry.LoopTillEnd)
            {
                info += " • Loops";
            }

            info += entry.TimeLimited ? " • Time-limited" : " • Untimed";

            var infoRect = new Rect(rect.x, rect.yMax - EditorGUIUtility.singleLineHeight + 2f, rect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(infoRect, info, EditorStyles.miniLabel);
        }

        private static void DrawStepBar(Rect rect, TimelineEntry entry, float totalDuration, int index, int totalSteps)
        {
            var fallbackWidth = rect.width / Mathf.Max(totalSteps, 1);
            var startX = entry.StartTime.HasValue
                ? rect.x + Mathf.Clamp01(entry.StartTime.Value / totalDuration) * rect.width
                : rect.x + index * fallbackWidth;

            var width = entry.Duration.HasValue
                ? Mathf.Max(8f, (entry.Duration.Value / totalDuration) * rect.width)
                : Mathf.Max(8f, fallbackWidth * 0.6f);

            var barRect = new Rect(startX, rect.y, Mathf.Min(width, rect.xMax - startX), rect.height);

            var color = entry.Blocking
                ? new Color(0.26f, 0.55f, 0.95f, 0.9f)
                : new Color(0.3f, 0.8f, 0.45f, 0.9f);

            if (!entry.Duration.HasValue)
            {
                color = Color.Lerp(color, new Color(0.35f, 0.35f, 0.35f, 0.9f), 0.35f);
            }

            EditorGUI.DrawRect(barRect, color);

            if (entry.LoopTillEnd)
            {
                Handles.color = new Color(0.98f, 0.98f, 0.98f, 0.7f);
                var y = barRect.y + barRect.height * 0.5f;
                Handles.DrawLine(new Vector2(barRect.x + 2f, y), new Vector2(barRect.xMax - 2f, y));
            }

            if (!entry.Duration.HasValue)
            {
                Handles.color = new Color(0.1f, 0.1f, 0.1f, 0.7f);
                var centerY = barRect.y + barRect.height * 0.5f;
                Handles.DrawDottedLine(new Vector2(barRect.x + 2f, centerY), new Vector2(barRect.xMax - 2f, centerY), 3f);
            }

            var label = BuildTimingLabel(entry);
            var labelStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                normal = { textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black }
            };

            GUI.Label(barRect, label, labelStyle);
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
