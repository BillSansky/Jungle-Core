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

                var stepsCopy = property.FindPropertyRelative(nameof(ActionSequence.Steps)).Copy();
                this.TrackPropertyValue(stepsCopy, _ => Refresh());

                var modeCopy = property.FindPropertyRelative(nameof(ActionSequence.Mode)).Copy();
                this.TrackPropertyValue(modeCopy, _ => Refresh());

                var sequenceLimitCopy = property.FindPropertyRelative(nameof(ActionSequence.SequenceTimeLimit)).Copy();
                this.TrackPropertyValue(sequenceLimitCopy, _ => Refresh());

                var sequenceStartDelayCopy = property.FindPropertyRelative(nameof(ActionSequence.StartDelay)).Copy();
                this.TrackPropertyValue(sequenceStartDelayCopy, _ => Refresh());
            }

            private void Refresh()
            {
                serializedObject.UpdateIfRequiredOrScript();

                var rootProperty = serializedObject.FindProperty(propertyPath);
                var stepsProp = rootProperty.FindPropertyRelative(nameof(ActionSequence.Steps));
                var modeProp = rootProperty.FindPropertyRelative(nameof(ActionSequence.Mode));
                var sequenceTimeLimitProp = rootProperty.FindPropertyRelative(nameof(ActionSequence.SequenceTimeLimit));
                var sequenceStartDelayProp = rootProperty.FindPropertyRelative(nameof(ActionSequence.StartDelay));

                rowsContainer.Clear();
                currentTimelineWidth = MinimumTimelineWidth;

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
                    var sequenceStartDelay = sequenceStartDelayProp.floatValue;
                    var entries = BuildEntries(stepsProp, sequenceStartDelay);
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

            private void BuildRows(List<TimelineEntry> entries, float totalDuration)
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

                    maxEnd = Mathf.Max(maxEnd, start + duration);
                }

                var usableDuration = Mathf.Max(Mathf.Max(totalDuration, maxEnd), 1f);
                currentTimelineWidth = Mathf.Max(MinimumTimelineWidth, usableDuration * PixelsPerSecond);

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

                    var bar = CreateBar(entry, out var label);
                    barColumn.Add(bar);

                    SetupBarInteractions(bar, label, entry);

                    row.Add(labelColumn);
                    row.Add(barColumn);
                    rowsContainer.Add(row);
                }
            }

            private static float[] CalculateFallbackStartTimes(IReadOnlyList<TimelineEntry> entries, float fallbackWidth)
            {
                var result = new float[entries.Count];
                var fallbackPosition = 0f;

                for (var i = 0; i < entries.Count; i++)
                {
                    var entry = entries[i];

                    float fraction;
                    if (entry.StartTime.HasValue)
                    {

                        var value = Mathf.Max(0f, entry.StartTime.Value);
                        result[i] = value;
                        lastKnownStart = value;
                        hasLastKnownStart = true;

                        fraction = Mathf.Clamp01(totalDuration > 0f ? entry.StartTime.Value / totalDuration : 0f);
                        fallbackPosition = Mathf.Max(fallbackPosition, fraction);

                    }
                    else
                    {

                        var fallback = Mathf.Max(0f, i * fallbackWidth);
                        result[i] = fallback;
                        lastKnownStart = fallback;
                        hasLastKnownStart = true;

                        fraction = fallbackPosition;

                    }

                    result[i] = fraction;

                    if (entry.Blocking)
                    {
                        fallbackPosition = Mathf.Clamp01(fraction + fallbackWidthFraction);
                    }
                }

                return result;
            }

            private VisualElement CreateBar(TimelineEntry entry, out Label label)
            {
                var baseColor = entry.Blocking
                    ? new Color(0.26f, 0.55f, 0.95f, 0.9f)
                    : new Color(0.3f, 0.8f, 0.45f, 0.9f);

                if (!entry.Duration.HasValue)
                {
                    baseColor = Color.Lerp(baseColor, new Color(0.35f, 0.35f, 0.35f, 0.9f), 0.35f);
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
                bar.style.cursor = new StyleCursor(MouseCursor.MoveArrow);

                UpdateBarVisual(bar, entry);

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

                label = new Label(BuildTimingLabel(entry.StartTime, entry.Duration, entry));
                label.style.unityTextAlign = TextAnchor.MiddleCenter;
                label.style.fontSize = 11;
                label.style.color = Color.white;
                label.style.unityFontStyleAndWeight = FontStyle.Bold;
                label.style.whiteSpace = WhiteSpace.Normal;

                bar.Add(label);

                return bar;
            }

            private void SetupBarInteractions(VisualElement bar, Label label, TimelineEntry entry)
            {
                if (entry.BaseStartTime.HasValue)
                {
                    bar.RegisterCallback<PointerDownEvent>(evt =>
                    {
                        if (evt.button != 0)
                        {
                            return;
                        }

                        evt.StopPropagation();
                        BeginDrag(entry, DragMode.MoveStart, bar, label, evt.pointerId, evt.position, entry.StartDelay, bar);
                    });

                    bar.RegisterCallback<PointerMoveEvent>(evt =>
                    {
                        if (currentDrag == null || currentDrag.Entry != entry || currentDrag.Mode != DragMode.MoveStart || evt.pointerId != currentDrag.PointerId)
                        {
                            return;
                        }

                        evt.StopPropagation();
                        UpdateMoveDrag(evt.position);
                    });

                    bar.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        if (currentDrag == null || evt.pointerId != currentDrag.PointerId)
                        {
                            return;
                        }

                        evt.StopPropagation();
                        EndDrag(true);
                    });

                    bar.RegisterCallback<PointerCaptureOutEvent>(_ =>
                    {
                        if (currentDrag != null && currentDrag.Entry == entry && currentDrag.Mode == DragMode.MoveStart)
                        {
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
                    handle.style.width = 6f;
                    handle.style.backgroundColor = new Color(1f, 1f, 1f, 0.3f);
                    handle.style.cursor = new StyleCursor(MouseCursor.ResizeHorizontal);

                    bar.Add(handle);

                    handle.RegisterCallback<PointerDownEvent>(evt =>
                    {
                        if (evt.button != 0)
                        {
                            return;
                        }

                        evt.StopPropagation();
                        BeginDrag(entry, DragMode.ResizeDuration, bar, label, evt.pointerId, evt.position, entry.DisplayDuration, handle);
                    });

                    handle.RegisterCallback<PointerMoveEvent>(evt =>
                    {
                        if (currentDrag == null || currentDrag.Entry != entry || currentDrag.Mode != DragMode.ResizeDuration || evt.pointerId != currentDrag.PointerId)
                        {
                            return;
                        }

                        evt.StopPropagation();
                        UpdateResizeDrag(evt.position);
                    });

                    handle.RegisterCallback<PointerUpEvent>(evt =>
                    {
                        if (currentDrag == null || evt.pointerId != currentDrag.PointerId)
                        {
                            return;
                        }

                        evt.StopPropagation();
                        EndDrag(true);
                    });

                    handle.RegisterCallback<PointerCaptureOutEvent>(_ =>
                    {
                        if (currentDrag != null && currentDrag.Entry == entry && currentDrag.Mode == DragMode.ResizeDuration)
                        {
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
                if (drag == null)
                {
                    return;
                }

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

            private void UpdateResizeDrag(Vector2 position)
            {
                var drag = currentDrag;
                if (drag == null)
                {
                    return;
                }

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

            private void EndDrag(bool refresh)
            {
                if (currentDrag == null)
                {
                    return;
                }

                if (currentDrag.PointerOwner.HasPointerCapture(currentDrag.PointerId))
                {
                    currentDrag.PointerOwner.ReleasePointer(currentDrag.PointerId);
                }

                currentDrag = null;

                if (refresh)
                {
                    Refresh();
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
                if (entry.LoopTillEnd)
                {
                    info += " • Loops";
                }

                info += entry.TimeLimited ? " • Time-limited" : " • Untimed";
                return info;
            }
        }

        private static List<TimelineEntry> BuildEntries(SerializedProperty stepsProp, float sequenceStartDelay)
        {
            var entries = new List<TimelineEntry>(stepsProp.arraySize);
            var currentTime = Mathf.Max(0f, sequenceStartDelay);
            var timeKnown = true;

            for (var i = 0; i < stepsProp.arraySize; i++)
            {
                var stepProp = stepsProp.GetArrayElementAtIndex(i);
                var blocking = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.blocking)).boolValue;
                var loopTillEnd = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.loopTillEnd)).boolValue;
                var timeLimitedProp = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.timeLimited));
                var timeLimited = timeLimitedProp.boolValue;
                var timeLimitProp = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.timeLimit));
                var startDelayProp = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.startDelay));
                var actionProp = stepProp.FindPropertyRelative(nameof(ActionSequence.Step.Action));

                var startDelay = Mathf.Max(0f, startDelayProp.floatValue);
                float? baseStartTime = timeKnown ? currentTime : (float?)null;
                float? startTime = baseStartTime.HasValue ? baseStartTime.Value + startDelay : (float?)null;

                float? duration = null;
                if (timeLimited)
                {
                    var limitValue = timeLimitProp.floatValue;
                    if (limitValue > 0f)
                    {
                        duration = limitValue;
                    }
                }

                entries.Add(new TimelineEntry
                {
                    DisplayName = BuildStepDisplayName(actionProp, i),
                    Blocking = blocking,
                    LoopTillEnd = loopTillEnd,
                    TimeLimited = timeLimited,
                    BaseStartTime = baseStartTime,
                    StartDelay = startDelay,
                    StartTime = startTime,
                    Duration = duration,
                    StartDelayProperty = startDelayProp,
                    TimeLimitProperty = timeLimitProp,
                    DurationEditable = timeLimited
                });

                if (timeKnown && baseStartTime.HasValue)
                {
                    if (blocking)
                    {
                        currentTime = baseStartTime.Value + startDelay;
                        if (duration.HasValue)
                        {
                            currentTime += duration.Value;
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

        private static float DetermineTimelineDuration(List<TimelineEntry> entries, SerializedProperty modeProp, SerializedProperty sequenceTimeLimitProp)
        {
            var max = 0f;

            foreach (var entry in entries)
            {
                if (entry.StartTime.HasValue)
                {
                    max = Mathf.Max(max, entry.StartTime.Value);
                    if (entry.Duration.HasValue)
                    {
                        max = Mathf.Max(max, entry.StartTime.Value + entry.Duration.Value);
                    }
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

        private static string BuildTimingLabel(float? startTime, float? duration, TimelineEntry entry)
        {
            var startLabel = startTime.HasValue
                ? $"Start T+{startTime.Value:0.##}s"
                : "Start T+?";

            string durationLabel;
            if (duration.HasValue)
            {
                durationLabel = duration.Value > 0f
                    ? $"Runs {duration.Value:0.##}s"
                    : "Runs 0s";
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
            public float? BaseStartTime;
            public float StartDelay;
            public float? StartTime;
            public float? Duration;
            public float DisplayStart;
            public float DisplayDuration;
            public bool DurationEditable;
            public SerializedProperty StartDelayProperty;
            public SerializedProperty TimeLimitProperty;
        }
    }
}
#endif
