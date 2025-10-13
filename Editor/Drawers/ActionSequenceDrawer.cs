#if UNITY_EDITOR
using System.Collections.Generic;
using System.Diagnostics;
using Jungle.Actions;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.UIElements.Cursor;
using Debug = UnityEngine.Debug;

namespace Jungle.Editor
{
    [CustomPropertyDrawer(typeof(SequenceAction))]
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

            var hasSequenceTimeLimitProp = property.FindPropertyRelative(nameof(SequenceAction.hasSequenceTimeLimit));
            var sequenceTimeLimitProp = property.FindPropertyRelative(nameof(SequenceAction.SequenceTimeLimit));
            var stepsProp = property.FindPropertyRelative(nameof(SequenceAction.Steps));

            foldout.Add(new PropertyField(hasSequenceTimeLimitProp));
            foldout.Add(new PropertyField(sequenceTimeLimitProp));
            foldout.Add(new PropertyField(stepsProp));

            var timelineFoldout = new Foldout
            {
                text = "Timeline",
                value = false
            };

            var timeline = new ActionSequenceTimeline(property);
            timelineFoldout.Add(timeline);
            foldout.Add(timelineFoldout);

            root.Bind(property.serializedObject);
            return root;
        }


        private sealed class ActionSequenceTimeline : VisualElement
        {
            private const float PixelsPerSecond = 80f;
            private const float MinimumTimelineWidth = 480f;
            private const float MinimumBarWidth = 12f;

            private readonly SerializedObject serializedObject;
            private readonly string propertyPath;

            private readonly Label headerLabel;
            private readonly VisualElement legendContainer;
            private readonly ScrollView timelineScrollView;
            private readonly VisualElement rowsContainer;
            private readonly Label modeInfoLabel;

            private float currentTimelineWidth = MinimumTimelineWidth;
            private DragState currentDrag;

            private sealed class DragState
            {
                public TimelineEntry Entry;
                public DragMode Mode;
                public VisualElement PointerOwner;
                public VisualElement BarElement;
                public Label LabelElement;
                public int PointerId;
                public Vector2 StartPosition;
                public float StartValue;
                public float BaseStartTime;
            }

            private enum DragMode
            {
                MoveStart,
                ResizeDuration
            }

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

                timelineScrollView = new ScrollView(ScrollViewMode.Horizontal)
                {
                    horizontalScrollerVisibility = ScrollerVisibility.Auto,
                    verticalScrollerVisibility = ScrollerVisibility.Hidden
                };
                timelineScrollView.style.flexGrow = 1f;
                timelineScrollView.style.marginTop = 2f;

                rowsContainer = new VisualElement();
                rowsContainer.style.flexDirection = FlexDirection.Column;
                rowsContainer.style.flexGrow = 1f;

                timelineScrollView.Add(rowsContainer);

                modeInfoLabel = new Label();
                modeInfoLabel.style.unityFontStyleAndWeight = FontStyle.Italic;
                modeInfoLabel.style.marginTop = 6f;
                modeInfoLabel.style.fontSize = 11;
                modeInfoLabel.style.color = EditorGUIUtility.isProSkin ? new Color(0.8f, 0.8f, 0.8f) : new Color(0.2f, 0.2f, 0.2f);

                Add(headerLabel);
                Add(legendContainer);
                Add(timelineScrollView);
                Add(modeInfoLabel);

                CreateLegend();
                Refresh();

                var propertyCopy = property.Copy();
                this.TrackPropertyValue(propertyCopy, _ => Refresh());

                var stepsCopy = property.FindPropertyRelative(nameof(SequenceAction.Steps)).Copy();
                this.TrackPropertyValue(stepsCopy, _ => Refresh());

                var hasSequenceTimeLimitCopy = property.FindPropertyRelative(nameof(SequenceAction.hasSequenceTimeLimit)).Copy();
                this.TrackPropertyValue(hasSequenceTimeLimitCopy, _ => Refresh());

                var sequenceLimitCopy = property.FindPropertyRelative(nameof(SequenceAction.SequenceTimeLimit)).Copy();
                this.TrackPropertyValue(sequenceLimitCopy, _ => Refresh());
            }

            private void Refresh()
            {
                serializedObject.UpdateIfRequiredOrScript();

                var rootProperty = serializedObject.FindProperty(propertyPath);
                var stepsProp = rootProperty.FindPropertyRelative(nameof(SequenceAction.Steps));
                var hasSequenceTimeLimitProp = rootProperty.FindPropertyRelative(nameof(SequenceAction.hasSequenceTimeLimit));
                var sequenceTimeLimitProp = rootProperty.FindPropertyRelative(nameof(SequenceAction.SequenceTimeLimit));

                rowsContainer.Clear();
                currentTimelineWidth = MinimumTimelineWidth;

                var hasSequenceTimeLimit = hasSequenceTimeLimitProp.boolValue;

                if (stepsProp.arraySize == 0)
                {
                    var empty = new Label("No steps defined.");
                    empty.style.unityTextAlign = TextAnchor.MiddleLeft;
                    empty.style.color = EditorGUIUtility.isProSkin ? new Color(0.7f, 0.7f, 0.7f) : new Color(0.3f, 0.3f, 0.3f);

                    var messageRow = new VisualElement();
                    messageRow.style.minWidth = LabelColumnWidth + currentTimelineWidth;
                    messageRow.style.paddingLeft = 4f;
                    messageRow.style.paddingTop = 6f;
                    messageRow.style.paddingBottom = 6f;
                    messageRow.Add(empty);

                    rowsContainer.Add(messageRow);
                }
                else
                {
                    var entries = BuildEntries(stepsProp, 0f);
                    var totalDuration = DetermineTimelineDuration(entries, hasSequenceTimeLimitProp, sequenceTimeLimitProp);
                    var sequenceTimeLimit = sequenceTimeLimitProp.floatValue;
                    BuildRows(entries, totalDuration, sequenceTimeLimit > 0f ? (float?)sequenceTimeLimit : null, hasSequenceTimeLimit);
                }

                if (hasSequenceTimeLimit && sequenceTimeLimitProp.floatValue > 0f)
                {
                    var limit = sequenceTimeLimitProp.floatValue;
                    modeInfoLabel.text = $"Sequence runs once with time limit of {limit:0.##}s.";
                }
                else
                {
                    modeInfoLabel.text = "Sequence runs once through all steps.";
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

            private void BuildRows(List<TimelineEntry> entries, float totalDuration, float? sequenceTimeLimit, bool hasSequenceTimeLimit)
            {
                var fallbackWidth = entries.Count > 0 ? Mathf.Max(totalDuration / entries.Count, 1f) : 1f;
                var fallbackStartTimes = CalculateFallbackStartTimes(entries, fallbackWidth);
                var fallbackDuration = Mathf.Max(fallbackWidth * 0.6f, 0.25f);

                var maxEnd = 0f;
                for (var i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];
                    var start = entry.StartTime ?? fallbackStartTimes[i];
                    var duration = entry.Duration ?? fallbackDuration;

                    entry.DisplayStart = start;
                    entry.DisplayDuration = duration;

                    // Calculate the actual end time for looped steps
                    if (entry.LoopsActive && entry.BaseDuration.HasValue)
                    {
                        if (entry.LoopMode == SequenceAction.StepLoopMode.Limited)
                        {
                            // For limited loops, the end is start + (baseDuration * loopCount)
                            maxEnd = Mathf.Max(maxEnd, start + (entry.BaseDuration.Value * entry.LoopCount));
                        }
                        else if (entry.LoopMode == SequenceAction.StepLoopMode.Infinite)
                        {
                            // For infinite loops, extend to sequence time limit if available
                            if (hasSequenceTimeLimit && sequenceTimeLimit.HasValue && sequenceTimeLimit.Value > 0f)
                            {
                                maxEnd = Mathf.Max(maxEnd, sequenceTimeLimit.Value);
                            }
                            else
                            {
                                // Otherwise, show at least 5 iterations or until totalDuration
                                var estimatedEnd = start + (entry.BaseDuration.Value * 5f);
                                maxEnd = Mathf.Max(maxEnd, Mathf.Min(estimatedEnd, totalDuration * 1.5f));
                            }
                        }
                    }
                    else
                    {
                        maxEnd = Mathf.Max(maxEnd, start + duration);
                    }
                }

                var usableDuration = Mathf.Max(Mathf.Max(totalDuration, maxEnd), 1f);
                currentTimelineWidth = Mathf.Max(MinimumTimelineWidth, usableDuration * PixelsPerSecond);

                // Create a container for the timeline area with time limit overlay
                var timelineContainer = new VisualElement();
                timelineContainer.style.position = Position.Relative;
                timelineContainer.style.flexDirection = FlexDirection.Column;
                timelineContainer.style.flexGrow = 1f;

                // Add global sequence time limit indicator (rendered across all rows)
                if (hasSequenceTimeLimit && sequenceTimeLimit.HasValue && sequenceTimeLimit.Value > 0f)
                {
                    var limitLine = new VisualElement();
                    limitLine.style.position = Position.Absolute;
                    limitLine.style.left = LabelColumnWidth + (sequenceTimeLimit.Value * PixelsPerSecond);
                    limitLine.style.top = 0f;
                    limitLine.style.bottom = 0f;
                    limitLine.style.width = 2f;
                    limitLine.style.backgroundColor = new Color(1f, 0.3f, 0.3f, 0.8f);
                    timelineContainer.Add(limitLine);

                    var limitLabel = new Label($"Sequence Limit: {sequenceTimeLimit.Value:0.##}s");
                    limitLabel.style.position = Position.Absolute;
                    limitLabel.style.left = LabelColumnWidth + (sequenceTimeLimit.Value * PixelsPerSecond) + 4f;
                    limitLabel.style.top = 2f;
                    limitLabel.style.fontSize = 10;
                    limitLabel.style.color = new Color(1f, 0.3f, 0.3f, 0.9f);
                    limitLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                    timelineContainer.Add(limitLabel);
                }

                for (var i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];

                    var row = new VisualElement();
                    row.style.flexDirection = FlexDirection.Row;
                    row.style.alignItems = Align.Stretch;
                    row.style.marginBottom = 4f;
                    row.style.minWidth = LabelColumnWidth + currentTimelineWidth + 16f;

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
                    barColumn.style.width = currentTimelineWidth;
                    barColumn.style.flexShrink = 0f;
                    barColumn.style.height = 40f;
                    barColumn.style.position = Position.Relative;
                    barColumn.style.backgroundColor = EditorGUIUtility.isProSkin ? new Color(0.18f, 0.18f, 0.18f) : new Color(0.88f, 0.88f, 0.88f);
                    barColumn.style.borderBottomLeftRadius = 3f;
                    barColumn.style.borderBottomRightRadius = 3f;
                    barColumn.style.borderTopLeftRadius = 3f;
                    barColumn.style.borderTopRightRadius = 3f;
                    barColumn.style.overflow = Overflow.Hidden;

                    // Create bars for loop iterations or single bar for non-looped steps
                    if (entry.LoopsActive && entry.BaseDuration.HasValue && entry.LoopMode == SequenceAction.StepLoopMode.Limited)
                    {
                        // Create individual bars for each loop iteration on the same horizontal line
                        var iterationStart = entry.DisplayStart;
                        for (int loopIndex = 0; loopIndex < entry.LoopCount; loopIndex++)
                        {
                            var iterationEntry = CreateLoopIterationEntry(entry, loopIndex, iterationStart);
                            var bar = CreateBar(iterationEntry, out var label);
                            barColumn.Add(bar);

                            if (loopIndex == 0)
                            {
                                SetupBarInteractions(bar, label, entry);
                            }

                            iterationStart += entry.BaseDuration.Value;
                        }
                    }
                    else if (entry.LoopsActive && entry.LoopMode == SequenceAction.StepLoopMode.Infinite)
                    {
                        // For infinite loops, show iterations until time limit or end of timeline
                        var iterationStart = entry.DisplayStart;
                        var endPoint = usableDuration;

                        // If there's a sequence time limit, use that as the end point
                        if (hasSequenceTimeLimit && sequenceTimeLimit.HasValue && sequenceTimeLimit.Value > 0f)
                        {
                            endPoint = sequenceTimeLimit.Value;
                        }

                        var maxIterations = 0;
                        if (entry.BaseDuration.HasValue && entry.BaseDuration.Value > 0f)
                        {
                            maxIterations = Mathf.CeilToInt((endPoint - entry.DisplayStart) / entry.BaseDuration.Value);
                            maxIterations = Mathf.Max(1, Mathf.Min(maxIterations, 50)); // Cap at 50 iterations for performance
                        }

                        for (int loopIndex = 0; loopIndex < maxIterations; loopIndex++)
                        {
                            // Stop if we've reached the end point
                            if (iterationStart >= endPoint)
                            {
                                break;
                            }

                            var iterationEntry = CreateLoopIterationEntry(entry, loopIndex, iterationStart);

                            // Clamp the duration if it would exceed the end point
                            if (iterationEntry.BaseDuration.HasValue && iterationStart + iterationEntry.BaseDuration.Value > endPoint)
                            {
                                iterationEntry.Duration = endPoint - iterationStart;
                                iterationEntry.DisplayDuration = iterationEntry.Duration.Value;
                            }

                            var bar = CreateBar(iterationEntry, out var label);
                            barColumn.Add(bar);

                            if (loopIndex == 0)
                            {
                                SetupBarInteractions(bar, label, entry);
                            }

                            if (entry.BaseDuration.HasValue)
                            {
                                iterationStart += entry.BaseDuration.Value;
                            }
                            else
                            {
                                break; // Can't continue without knowing duration
                            }
                        }
                    }
                    else
                    {
                        // Single bar for non-looped steps
                        var bar = CreateBar(entry, out var label);
                        barColumn.Add(bar);
                        SetupBarInteractions(bar, label, entry);
                    }

                    row.Add(labelColumn);
                    row.Add(barColumn);
                    timelineContainer.Add(row);
                }

                rowsContainer.Add(timelineContainer);
            }

            private static TimelineEntry CreateLoopIterationEntry(TimelineEntry originalEntry, int iterationIndex, float iterationStart)
            {
                var iterationEntry = new TimelineEntry
                {
                    DisplayName = originalEntry.DisplayName + $" #{iterationIndex + 1}",
                    Blocking = originalEntry.Blocking,
                    LoopMode = originalEntry.LoopMode,
                    LoopsActive = originalEntry.LoopsActive,
                    LoopCount = originalEntry.LoopCount,
                    TimeLimited = originalEntry.TimeLimited,
                    BaseStartTime = originalEntry.BaseStartTime,
                    StartDelay = originalEntry.StartDelay,
                    StartTime = iterationStart,
                    Duration = originalEntry.BaseDuration,
                    BaseDuration = originalEntry.BaseDuration,
                    DisplayStart = iterationStart,
                    DisplayDuration = originalEntry.BaseDuration ?? 0.5f,
                    StartDelayProperty = originalEntry.StartDelayProperty,
                    TimeLimitProperty = originalEntry.TimeLimitProperty,
                    DurationEditable = originalEntry.DurationEditable,
                    IsLoopIteration = true,
                    LoopIterationIndex = iterationIndex
                };

                return iterationEntry;
            }

            // Fixed, deterministic fallback placement without undefined variables.
            private static float[] CalculateFallbackStartTimes(IReadOnlyList<TimelineEntry> entries, float fallbackWidth)
            {
                var result = new float[entries.Count];
                var cursor = 0f; // advances when blocking items consume time

                for (var i = 0; i < entries.Count; i++)
                {
                    var e = entries[i];

                    if (e.StartTime.HasValue)
                    {
                        var start = Mathf.Max(0f, e.StartTime.Value);
                        result[i] = start;

                        if (e.Blocking)
                        {
                            var dur = e.Duration.HasValue ? Mathf.Max(0f, e.Duration.Value) : Mathf.Max(0.25f, fallbackWidth * 0.6f);
                            cursor = Mathf.Max(cursor, start + dur);
                        }
                    }
                    else
                    {
                        // Add delay visualization: non-blocking items with delays start at cursor + delay
                        var baseStart = Mathf.Max(0f, cursor);
                        var delayOffset = e.StartDelay > 0f ? e.StartDelay : 0f;
                        var start = baseStart + delayOffset;
                        result[i] = start;

                        if (e.Blocking)
                        {
                            var dur = e.Duration.HasValue ? Mathf.Max(0f, e.Duration.Value) : Mathf.Max(0.25f, fallbackWidth * 0.6f);
                            cursor = start + dur;
                        }
                        else if (e.StartDelay > 0f)
                        {
                            // For parallel steps with delays, ensure cursor accounts for the delay
                            cursor = Mathf.Max(cursor, start);
                        }
                    }
                }

                return result;
            }

            private VisualElement CreateBar(TimelineEntry entry, out Label label)
            {
                // Determine base color from properties
                var baseColor = entry.Blocking
                    ? new Color(0.26f, 0.55f, 0.95f, 0.9f)  // Blue for blocking
                    : new Color(0.3f, 0.8f, 0.45f, 0.9f);   // Green for parallel

                // Looping steps get orange tint
                if (entry.LoopsActive)
                {
                    baseColor = new Color(0.95f, 0.67f, 0.25f, 0.9f);
                }

                // Loop iterations get progressive fade for infinite loops
                if (entry.IsLoopIteration && entry.LoopMode == SequenceAction.StepLoopMode.Infinite)
                {
                    var fadeAmount = Mathf.Min(0.6f, entry.LoopIterationIndex * 0.1f);
                    baseColor = Color.Lerp(baseColor, new Color(0.5f, 0.5f, 0.5f, 0.4f), fadeAmount);
                }
                else if (entry.IsLoopIteration)
                {
                    // Limited loop iterations get slight dimming
                    baseColor = Color.Lerp(baseColor, new Color(0.5f, 0.5f, 0.5f, 0.9f), 0.15f);
                }

                // Unknown duration gets gray overlay
                if (!entry.Duration.HasValue)
                {
                    baseColor = Color.Lerp(baseColor, new Color(0.4f, 0.4f, 0.4f, 0.9f), 0.5f);
                }

                // Time-limited steps get a slight red tint
                if (entry.TimeLimited)
                {
                    baseColor = Color.Lerp(baseColor, new Color(1f, 0.4f, 0.4f, 0.9f), 0.15f);
                }

                var bar = new VisualElement();
                bar.style.position = Position.Absolute;
                bar.style.top = 6f;
                bar.style.bottom = 6f;
                bar.style.minWidth = MinimumBarWidth;
                bar.style.backgroundColor = baseColor;
                bar.style.borderBottomLeftRadius = 3f;
                bar.style.borderBottomRightRadius = 3f;
                bar.style.borderTopLeftRadius = 3f;
                bar.style.borderTopRightRadius = 3f;
                bar.style.borderBottomWidth = 1f;
                bar.style.borderTopWidth = 1f;
                bar.style.borderLeftWidth = 1f;
                bar.style.borderRightWidth = 1f;
                bar.style.borderBottomColor = new Color(0f, 0f, 0f, 0.3f);
                bar.style.borderTopColor = new Color(0f, 0f, 0f, 0.3f);
                bar.style.borderLeftColor = new Color(0f, 0f, 0f, 0.3f);
                bar.style.borderRightColor = new Color(0f, 0f, 0f, 0.3f);
                bar.style.cursor = new StyleCursor(StyleKeyword.Auto);

                UpdateBarVisual(bar, entry);

                if (entry.LoopsActive && !entry.IsLoopIteration)
                {
                    var loopStripe = new VisualElement
                    {
                        style =
                        {
                            position = Position.Absolute,
                            left = 2f,
                            right = 2f,
                            top = 0f,
                            height = 3f,
                            backgroundColor = new Color(1f, 1f, 1f, 0.85f),
                            borderBottomLeftRadius = 1f,
                            borderBottomRightRadius = 1f
                        }
                    };
                    bar.Add(loopStripe);
                }

                // Add iteration number for loop iterations
                if (entry.IsLoopIteration)
                {
                    var iterationLabel = new Label($"{entry.LoopIterationIndex + 1}");
                    iterationLabel.style.position = Position.Absolute;
                    iterationLabel.style.right = 3f;
                    iterationLabel.style.top = 1f;
                    iterationLabel.style.fontSize = 9;
                    iterationLabel.style.color = new Color(1f, 1f, 1f, 0.8f);
                    iterationLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                    bar.Add(iterationLabel);
                }
                else if (entry.LoopsActive && entry.LoopMode == SequenceAction.StepLoopMode.Infinite)
                {
                    // Add infinity symbol for the first iteration of infinite loops
                    var infinityLabel = new Label("∞");
                    infinityLabel.style.position = Position.Absolute;
                    infinityLabel.style.right = 4f;
                    infinityLabel.style.top = 2f;
                    infinityLabel.style.fontSize = 14;
                    infinityLabel.style.color = new Color(1f, 1f, 1f, 0.9f);
                    infinityLabel.style.unityFontStyleAndWeight = FontStyle.Bold;
                    bar.Add(infinityLabel);
                }

                label = new Label(BuildTimingLabel(entry.StartTime, entry.Duration, entry));
                label.style.unityTextAlign = TextAnchor.MiddleCenter;
                label.style.fontSize = 11;
                label.style.color = Color.white;
                label.style.unityFontStyleAndWeight = FontStyle.Bold;
                label.style.whiteSpace = WhiteSpace.Normal;
                label.style.textShadow = new TextShadow
                {
                    offset = new Vector2(1f, 1f),
                    blurRadius = 2f,
                    color = new Color(0f, 0f, 0f, 0.5f)
                };

                bar.Add(label);

                return bar;
            }

            private void SetupBarInteractions(VisualElement bar, Label label, TimelineEntry entry)
            {
                if (entry.BaseStartTime.HasValue && !entry.IsLoopIteration && entry.StartDelayProperty != null)
                {
                    bar.RegisterCallback<PointerEnterEvent>(_ =>
                    {
                        if (currentDrag == null)
                        {
                            bar.style.borderBottomColor = new Color(1f, 1f, 1f, 0.6f);
                            bar.style.borderTopColor = new Color(1f, 1f, 1f, 0.6f);
                            bar.style.borderLeftColor = new Color(1f, 1f, 1f, 0.6f);
                            bar.style.borderRightColor = new Color(1f, 1f, 1f, 0.6f);
                        }
                    });

                    bar.RegisterCallback<PointerLeaveEvent>(_ =>
                    {
                        if (currentDrag == null)
                        {
                            bar.style.borderBottomColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderTopColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderLeftColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderRightColor = new Color(0f, 0f, 0f, 0.3f);
                        }
                    });

                    bar.RegisterCallback<PointerDownEvent>(evt =>
                    {
                        if (evt.button != 0) return;

                        evt.StopPropagation();
                        bar.style.borderBottomColor = new Color(1f, 1f, 0f, 0.9f);
                        bar.style.borderTopColor = new Color(1f, 1f, 0f, 0.9f);
                        bar.style.borderLeftColor = new Color(1f, 1f, 0f, 0.9f);
                        bar.style.borderRightColor = new Color(1f, 1f, 0f, 0.9f);
                        BeginDrag(entry, DragMode.MoveStart, bar, label, evt.pointerId, evt.position, entry.StartDelay, bar);
                    });

                    bar.RegisterCallback<PointerMoveEvent>(evt =>
                    {
                        var drag = currentDrag;
                        var shouldHandle =
                            drag != null &&
                            drag.Entry == entry &&
                            drag.Mode == DragMode.MoveStart &&
                            evt.pointerId == drag.PointerId;

                        if (shouldHandle)
                        {
                            evt.StopPropagation();
                            UpdateMoveDrag(evt.position);
                        }
                    });

                    bar.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        var drag = currentDrag;
                        var shouldHandle = drag != null && evt.pointerId == drag.PointerId;

                        if (shouldHandle)
                        {
                            evt.StopPropagation();
                            bar.style.borderBottomColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderTopColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderLeftColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderRightColor = new Color(0f, 0f, 0f, 0.3f);
                            EndDrag(true);
                        }
                    });

                    bar.RegisterCallback<PointerCaptureOutEvent>(_ =>
                    {
                        var drag = currentDrag;
                        var shouldHandle = drag != null && drag.Entry == entry && drag.Mode == DragMode.MoveStart;

                        if (shouldHandle)
                        {
                            bar.style.borderBottomColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderTopColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderLeftColor = new Color(0f, 0f, 0f, 0.3f);
                            bar.style.borderRightColor = new Color(0f, 0f, 0f, 0.3f);
                            EndDrag(false);
                        }
                    });
                }

                if (entry.DurationEditable)
                {
                    var handle = new VisualElement();
                    handle.style.position = Position.Absolute;
                    handle.style.right = 0f;
                    handle.style.top = 0f;
                    handle.style.bottom = 0f;
                    handle.style.width = 8f;
                    handle.style.backgroundColor = new Color(1f, 1f, 1f, 0.4f);
                    handle.style.cursor = new StyleCursor(StyleKeyword.Auto);
                    handle.style.borderTopRightRadius = 3f;
                    handle.style.borderBottomRightRadius = 3f;

                    bar.Add(handle);

                    handle.RegisterCallback<PointerEnterEvent>(_ =>
                    {
                        if (currentDrag == null)
                        {
                            handle.style.backgroundColor = new Color(1f, 1f, 1f, 0.7f);
                        }
                    });

                    handle.RegisterCallback<PointerLeaveEvent>(_ =>
                    {
                        if (currentDrag == null)
                        {
                            handle.style.backgroundColor = new Color(1f, 1f, 1f, 0.4f);
                        }
                    });

                    handle.RegisterCallback<PointerDownEvent>(evt =>
                    {
                        if (evt.button != 0) return;

                        evt.StopPropagation();
                        handle.style.backgroundColor = new Color(1f, 1f, 0f, 0.9f);
                        BeginDrag(entry, DragMode.ResizeDuration, bar, label, evt.pointerId, evt.position, entry.DisplayDuration, handle);
                    });

                    handle.RegisterCallback<PointerMoveEvent>(evt =>
                    {
                        var drag = currentDrag;
                        var shouldHandle =
                            drag != null &&
                            drag.Entry == entry &&
                            drag.Mode == DragMode.ResizeDuration &&
                            evt.pointerId == drag.PointerId;

                        if (shouldHandle)
                        {
                            evt.StopPropagation();
                            UpdateResizeDrag(evt.position);
                        }
                    });

                    handle.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        var drag = currentDrag;
                        var shouldHandle = drag != null && evt.pointerId == drag.PointerId;

                        if (shouldHandle)
                        {
                            evt.StopPropagation();
                            handle.style.backgroundColor = new Color(1f, 1f, 1f, 0.4f);
                            EndDrag(true);
                        }
                    });

                    handle.RegisterCallback<PointerCaptureOutEvent>(_ =>
                    {
                        var drag = currentDrag;
                        var shouldHandle = drag != null && drag.Entry == entry && drag.Mode == DragMode.ResizeDuration;

                        if (shouldHandle)
                        {
                            handle.style.backgroundColor = new Color(1f, 1f, 1f, 0.4f);
                            EndDrag(false);
                        }
                    });
                }
            }

            private void BeginDrag(TimelineEntry entry, DragMode mode, VisualElement bar, Label label, int pointerId, Vector2 position, float startValue, VisualElement pointerOwner)
            {
                currentDrag = new DragState
                {
                    Entry = entry,
                    Mode = mode,
                    PointerOwner = pointerOwner,
                    BarElement = bar,
                    LabelElement = label,
                    PointerId = pointerId,
                    StartPosition = position,
                    StartValue = startValue,
                    BaseStartTime = entry.BaseStartTime ?? 0f
                };

                pointerOwner.CapturePointer(pointerId);

                var targets = serializedObject.targetObjects;
                if (targets != null && targets.Length > 0)
                {
                    var description = mode == DragMode.MoveStart ? "Adjust Step Start" : "Adjust Step Duration";
                    Undo.RecordObjects(targets, description);
                }
            }

            private void UpdateMoveDrag(Vector2 position)
            {
                var drag = currentDrag;
                Debug.Assert(drag != null, "UpdateMoveDrag called without an active drag state");
                if (drag != null)
                {
                    var deltaPixels = position.x - drag.StartPosition.x;
                    var deltaSeconds = deltaPixels / PixelsPerSecond;
                    var newDelay = Mathf.Max(0f, drag.StartValue + deltaSeconds);

                    drag.Entry.StartDelayProperty.floatValue = newDelay;
                    serializedObject.ApplyModifiedProperties();

                    drag.Entry.StartDelay = newDelay;
                    var newStartTime = drag.BaseStartTime + newDelay;
                    drag.Entry.StartTime = newStartTime;
                    drag.Entry.DisplayStart = newStartTime;

                    UpdateBarVisual(drag.BarElement, drag.Entry);
                    drag.LabelElement.text = BuildTimingLabel(drag.Entry.StartTime, drag.Entry.Duration, drag.Entry);
                }
            }

            private void UpdateResizeDrag(Vector2 position)
            {
                var drag = currentDrag;
                Debug.Assert(drag != null, "UpdateResizeDrag called without an active drag state");
                if (drag != null)
                {
                    var deltaPixels = position.x - drag.StartPosition.x;
                    var deltaSeconds = deltaPixels / PixelsPerSecond;
                    var newDuration = Mathf.Max(0f, drag.StartValue + deltaSeconds);

                    drag.Entry.TimeLimitProperty.floatValue = newDuration;
                    serializedObject.ApplyModifiedProperties();

                    drag.Entry.Duration = newDuration;
                    drag.Entry.DisplayDuration = newDuration;

                    UpdateBarVisual(drag.BarElement, drag.Entry);
                    drag.LabelElement.text = BuildTimingLabel(drag.Entry.StartTime, drag.Entry.Duration, drag.Entry);
                }
            }

            private void EndDrag(bool refresh)
            {
                var drag = currentDrag;
                Debug.Assert(drag != null, "EndDrag called without an active drag state");
                if (drag != null)
                {
                    if (drag.PointerOwner.HasPointerCapture(drag.PointerId))
                    {
                        drag.PointerOwner.ReleasePointer(drag.PointerId);
                    }
                    currentDrag = null;

                    if (refresh)
                    {
                        Refresh();
                    }
                }
            }

            private static void UpdateBarVisual(VisualElement bar, TimelineEntry entry)
            {
                var startPixels = Mathf.Max(0f, entry.DisplayStart) * PixelsPerSecond;
                var widthPixels = Mathf.Max(MinimumBarWidth, Mathf.Max(0f, entry.DisplayDuration) * PixelsPerSecond);

                bar.style.left = startPixels;
                bar.style.width = widthPixels;
            }

            private static string BuildInfoLabel(TimelineEntry entry)
            {
                var info = entry.Blocking ? "Blocking" : "Parallel";

                if (entry.StartDelay > 0f)
                {
                    info += $" • Delay: {entry.StartDelay:0.##}s";
                }

                if (entry.LoopsActive)
                {
                    switch (entry.LoopMode)
                    {
                        case SequenceAction.StepLoopMode.Infinite:
                            info += " • Loops: ∞";
                            if (entry.BaseDuration.HasValue)
                            {
                                info += $" ({entry.BaseDuration.Value:0.##}s each)";
                            }
                            break;
                        case SequenceAction.StepLoopMode.Limited:
                            info += $" • Loops: {entry.LoopCount}x";
                            if (entry.BaseDuration.HasValue)
                            {
                                info += $" ({entry.BaseDuration.Value:0.##}s each)";
                            }
                            break;
                    }
                }

                if (entry.TimeLimited)
                {
                    var durationText = entry.LoopsActive && entry.Duration.HasValue ? 
                        $"{entry.Duration.Value:0.##}s total" : 
                        (entry.Duration.HasValue ? entry.Duration.Value.ToString("0.##") : "?") + "s";
                    info += $" • Time limit: {durationText}";
                }

                return info;
            }
        }

        private static List<TimelineEntry> BuildEntries(SerializedProperty stepsProp, float sequenceStartDelay)
        {
            var entries = new List<TimelineEntry>();
            var currentTime = Mathf.Max(0f, sequenceStartDelay);
            var timeKnown = true;

            for (var i = 0; i < stepsProp.arraySize; i++)
            {
                var stepProp = stepsProp.GetArrayElementAtIndex(i);
                var blocking = stepProp.FindPropertyRelative(nameof(SequenceAction.Step.blocking)).boolValue;
                var loopModeProp = stepProp.FindPropertyRelative(nameof(SequenceAction.Step.loopMode));
                var loopCountProp = stepProp.FindPropertyRelative(nameof(SequenceAction.Step.loopCount));
                var timeLimitedProp = stepProp.FindPropertyRelative(nameof(SequenceAction.Step.timeLimited));
                var timeLimited = timeLimitedProp.boolValue;
                var timeLimitProp = stepProp.FindPropertyRelative(nameof(SequenceAction.Step.timeLimit));
                var startDelayProp = stepProp.FindPropertyRelative(nameof(SequenceAction.Step.startDelay));
                var actionProp = stepProp.FindPropertyRelative(nameof(SequenceAction.Step.Action));

                var startDelay = Mathf.Max(0f, startDelayProp.floatValue);
                var loopMode = (SequenceAction.StepLoopMode)loopModeProp.enumValueIndex;
                var loopsActive = loopMode != SequenceAction.StepLoopMode.Once;
                var loopCount = Mathf.Max(1, loopCountProp.intValue);
                

                // Create a temporary step instance to use GetDuration method
                var tempStep = new SequenceAction.Step
                {
                    Action = actionProp.managedReferenceValue as IProcessAction,
                    timeLimited = timeLimited,
                    timeLimit = timeLimitProp.floatValue
                };

                float? baseDuration = tempStep.GetDuration();

                // Apply fallback logic if GetDuration returns null
                if (!baseDuration.HasValue)
                {
                    if (tempStep.Action == null)
                    {
                        baseDuration = 0.5f; // Default fallback for unassigned actions
                    }
                    else
                    {
                        baseDuration = 1f; // Fallback for actions without known duration
                    }
                }

                // Calculate total duration for looped steps
                float? totalDuration = baseDuration;
                if (loopsActive && baseDuration.HasValue)
                {
                    if (loopMode == SequenceAction.StepLoopMode.Limited)
                    {
                        totalDuration = baseDuration.Value * loopCount;
                    }
                    // For infinite loops, keep the single iteration duration for display
                }

                float? baseStartTime = timeKnown ? currentTime : (float?)null;
                float? startTime = baseStartTime.HasValue ? baseStartTime.Value + startDelay : (float?)null;

                var displayName = BuildStepDisplayName(actionProp, i);

                entries.Add(new TimelineEntry
                {
                    DisplayName = displayName,
                    Blocking = blocking,
                    LoopMode = loopMode,
                    LoopsActive = loopsActive,
                    LoopCount = loopCount,
                    TimeLimited = timeLimited,
                    BaseStartTime = baseStartTime,
                    StartDelay = startDelay,
                    StartTime = startTime,
                    Duration = totalDuration,
                    BaseDuration = baseDuration, // Store the single iteration duration
                    StartDelayProperty = startDelayProp,
                    TimeLimitProperty = timeLimitProp,
                    DurationEditable = timeLimited,
                    IsLoopIteration = false,
                    LoopIterationIndex = 0
                });

                if (timeKnown && baseStartTime.HasValue)
                {
                    if (blocking)
                    {
                        currentTime = baseStartTime.Value + startDelay;
                        if (totalDuration.HasValue)
                        {
                            currentTime += totalDuration.Value;
                        }
                        else
                        {
                            timeKnown = false;
                        }
                    }
                }
            }

            return entries;
        }

        private static float DetermineTimelineDuration(List<TimelineEntry> entries, SerializedProperty hasSequenceTimeLimitProp, SerializedProperty sequenceTimeLimitProp)
        {
            var max = 0f;

            foreach (var entry in entries)
            {
                // Calculate the end time for this entry
                if (entry.StartTime.HasValue && entry.Duration.HasValue)
                {
                    var endTime = entry.StartTime.Value + entry.Duration.Value;
                    max = Mathf.Max(max, endTime);
                }
                else if (entry.StartTime.HasValue)
                {
                    // If no duration is known, at least account for the start time
                    max = Mathf.Max(max, entry.StartTime.Value);
                }
            }

            if (hasSequenceTimeLimitProp.boolValue)
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

        private static string BuildTimingLabel(float? startTime, float? duration, TimelineEntry entry)
        {
            var startLabel = startTime.HasValue
                ? $"Start T+{startTime.Value:0.##}s"
                : "Start T+?";

            string durationLabel;
            if (duration.HasValue)
            {
                if (entry.IsLoopIteration)
                {
                    durationLabel = $"Iter {entry.LoopIterationIndex + 1}\n{duration.Value:0.##}s";
                }
                else if (entry.LoopsActive && entry.BaseDuration.HasValue && !entry.IsLoopIteration)
                {
                    if (entry.LoopMode == SequenceAction.StepLoopMode.Infinite)
                    {
                        durationLabel = $"∞ × {entry.BaseDuration.Value:0.##}s";
                    }
                    else if (entry.LoopMode == SequenceAction.StepLoopMode.Limited)
                    {
                        durationLabel = $"{entry.LoopCount}× {entry.BaseDuration.Value:0.##}s = {duration.Value:0.##}s";
                    }
                    else
                    {
                        durationLabel = duration.Value > 0f ? $"Runs {duration.Value:0.##}s" : "Runs 0s";
                    }
                }
                else
                {
                    durationLabel = duration.Value > 0f ? $"Runs {duration.Value:0.##}s" : "Runs 0s";
                }
            }
            else if (entry.Blocking)
            {
                durationLabel = "Duration ?";
            }
            else
            {
                durationLabel = "Runs alongside";
            }

            return entry.IsLoopIteration ? durationLabel : $"{startLabel}\n{durationLabel}";
        }

        private sealed class TimelineEntry
        {
            public string DisplayName = string.Empty;
            public bool Blocking;
            public SequenceAction.StepLoopMode LoopMode;
            public bool LoopsActive;
            public int LoopCount;
            public bool TimeLimited;
            public float? BaseStartTime;
            public float StartDelay;
            public float? StartTime;
            public float? Duration;
            public float? BaseDuration; // Single iteration duration for looped steps
            public float DisplayStart;
            public float DisplayDuration;
            public bool DurationEditable;
            public SerializedProperty StartDelayProperty;
            public SerializedProperty TimeLimitProperty;
            public bool IsLoopIteration;
            public int LoopIterationIndex;
        }
    }
}
#endif
