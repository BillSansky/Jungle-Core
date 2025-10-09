using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Event-driven sequence of child ProcessActions that runs once through all steps.
    /// - Progression listens to child ProcessCompleted/ProcessFailed (no polling).
    /// - Coroutine only services time limits (sequence & per-step).
    /// - Optional sequence time limit can be set to constrain the entire sequence duration.
    /// - For per-step timeout with finishExecutionOnEndTime=true: do NOT force-complete; instead suppress exactly one loop on next natural completion.
    /// </summary>
    [Serializable]
    public class ActionSequence : BeginCompleteAction
    {
        [SerializeField]
        public List<Step> Steps = new List<Step>();

        [Header("Sequence Settings")]
        [Tooltip("Enable to set a time limit for the entire sequence.")]
        public bool hasSequenceTimeLimit = false;

        [Tooltip("Time limit for the entire sequence in seconds.")]
        public float SequenceTimeLimit = 0f;

        /// <summary>True if the sequence itself is time-limited or any step has a time limit.</summary>
        public override bool IsTimed =>
            (hasSequenceTimeLimit && SequenceTimeLimit > 0f) ||
            (Steps != null && Steps.Exists(s => s.timeLimited && s.timeLimit > 0f));

        public override float Duration
        {
            get
            {
                // If sequence has a time limit, return the shorter of sequence time limit or total step duration
                float totalDuration = 0f;
                if (Steps != null && Steps.Count > 0)
                {
                    foreach (var step in Steps)
                    {
                        totalDuration += step.startDelay;
                        if (step.Action != null && step.Action.IsTimed)
                        {
                            totalDuration += step.Action.Duration;
                        }
                    }
                }

                if (hasSequenceTimeLimit && SequenceTimeLimit > 0f)
                {
                    return totalDuration > 0f ? Mathf.Min(totalDuration, SequenceTimeLimit) : SequenceTimeLimit;
                }

                return totalDuration;
            }
        }

        public enum StepLoopMode
        {
            Once,    // Execute only once
            Infinite,    // Infinite indefinitely
            Limited  // Infinite a specific number of times
        }

        [Serializable]
        public class Step
        {
            [JungleClassSelection] [SerializeReference]
            public ProcessAction Action;

            [Header("Infinite Settings")]
            [Tooltip("Controls how this step loops: Once (no loop), Infinite (infinite), or Limited (specific number of loops).")]
            public StepLoopMode loopMode = StepLoopMode.Once;

            [Tooltip("Number of times to execute this step when loopMode is Limited.")]
            [Min(1)]
            public int loopCount = 1;

            [Tooltip("If true, the next step waits for this one to finish (blocking). If false, the next step starts immediately (parallel).")]
            public bool blocking = true;

            [Header("Step Start Delay")]
            [Tooltip("Delay in seconds before this step begins after it becomes eligible.")]
            public float startDelay = 0f;

            [Header("Step Time Limit")]
            public bool timeLimited;

            [Tooltip("If true, when the time limit elapses we do NOT force-complete; we let it finish naturally and skip exactly one loop on completion. If false, we cancel on timeout.")]
            public bool finishExecutionOnEndTime;

            [Tooltip("Seconds allowed for this step if timeLimited is true.")]
            public float timeLimit = 0f;

            // Runtime (not serialized)
            [NonSerialized] internal float TimeLeft;
            [NonSerialized] internal bool Started;

            [NonSerialized] internal bool WaitingForStartDelay;
            [NonSerialized] internal float StartDelayRemaining;

            // Timeout behavior: after a timeout (finishExecutionOnEndTime == true),
            // allow the action to complete naturally, but skip exactly one loop on that completion.
            [NonSerialized] internal bool suppressLoopOnce;

            [NonSerialized] internal int loopsCompleted;

            // Event handlers to detach safely
            [NonSerialized] internal Action completedHandler;
            [NonSerialized] internal Action failedHandler;

            /// <summary>
            /// Gets the duration of this step based on time limit or action duration.
            /// Returns the shorter of the two if both are available.
            /// Returns null if no duration can be determined.
            /// </summary>
            public float? GetDuration()
            {
                float? actionDuration = null;
                float? stepTimeLimit = null;

                // Get action duration if available
                if (Action != null && Action.IsTimed && Action.Duration > 0f)
                {
                    actionDuration = Action.Duration;
                }

                // Get time limit if set
                if (timeLimited && timeLimit > 0f)
                {
                    stepTimeLimit = timeLimit;
                }

                // Return the shorter of the two, or whichever is available
                if (actionDuration.HasValue && stepTimeLimit.HasValue)
                {
                    return Mathf.Min(actionDuration.Value, stepTimeLimit.Value);
                }

                return actionDuration ?? stepTimeLimit;
            }
        }

        // Runtime state
        [NonSerialized] private bool running;
        [NonSerialized] private int currentIndex;
        [NonSerialized] private float sequenceTimeLeft;
        [NonSerialized] private readonly List<Step> parallelRunning = new List<Step>();
        // Coroutine handle
        [NonSerialized] private Coroutine tickRoutine;

        protected override void BeginProcessImpl()
        {
            if (Steps == null || Steps.Count == 0)
            {
                Complete();
                return;
            }

            running = true;
            currentIndex = 0;
            parallelRunning.Clear();

            InitializeStepRuntime();

            sequenceTimeLeft = (hasSequenceTimeLimit && SequenceTimeLimit > 0f)
                ? SequenceTimeLimit
                : float.PositiveInfinity;

            StartNextEligibleSteps();

            // drives only time limits via coroutine
            tickRoutine = CoroutineRunner.StartManagedCoroutine(TickRoutine());
        }

        protected override void CancelImpl()
        {
            running = false;

            if (tickRoutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(tickRoutine);
                tickRoutine = null;
            }

            foreach (var s in Steps)
            {
                DetachStepListeners(s);
                SafeCancel(s.Action);
                ResetStepRuntimeState(s, true);
            }

            parallelRunning.Clear();
        }

        protected override void CompleteImpl()
        {
            running = false;

            if (tickRoutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(tickRoutine);
                tickRoutine = null;
            }

            foreach (var s in Steps)
            {
                DetachStepListeners(s);
                ResetStepRuntimeState(s, true);
            }

            parallelRunning.Clear();
        }

        /// <summary>
        /// Coroutine that drives per-frame updates for time limits only.
        /// </summary>
        private IEnumerator TickRoutine()
        {
            while (running)
            {
                var deltaTime = Time.deltaTime;

                var startedSteps = UpdateStartDelays(deltaTime);

                if (startedSteps)
                {
                    StartNextEligibleSteps();
                }

                if (!ShouldPauseTimeLimits())
                {
                    ServiceTimeLimits(deltaTime);
                }

                yield return null;
            }
        }

        /// <summary>
        /// Decrements sequence and per-step timers and enforces timeouts.
        /// </summary>
        private void ServiceTimeLimits(float deltaTime)
        {
            if (!running) return;

            // Sequence time limiting (if enabled)
            if (hasSequenceTimeLimit && SequenceTimeLimit > 0f)
            {
                sequenceTimeLeft -= deltaTime;
                if (sequenceTimeLeft <= 0f)
                {
                    Complete();
                    return;
                }
            }

            // Step time limiting (for whatever is currently running)
            for (int i = parallelRunning.Count - 1; i >= 0; i--)
            {
                var step = parallelRunning[i];
                if (!(step.timeLimited && step.timeLimit > 0f))
                    continue;

                step.TimeLeft -= deltaTime;
                if (step.TimeLeft > 0f) continue;

                // Timeout reached
                if (step.finishExecutionOnEndTime)
                {
                    // Do NOT force-complete. Let it finish naturally,
                    // but skip one loop when it completes.
                    step.suppressLoopOnce = true;
                    // no immediate terminal handling; we keep it running
                }
                else
                {
                    // Cancel-on-timeout behavior:
                    SafeCancel(step.Action);
                    HandleStepTerminal(step);
                }

                // Reset the per-step timer so we don't keep firing every frame.
                // If we want "single-shot" timeout per run, set to +inf; otherwise re-arm.
                step.TimeLeft = float.PositiveInfinity;
            }
        }

        // ---------- progression helpers (event-driven) ----------

        private bool UpdateStartDelays(float deltaTime)
        {
            var anyElapsed = false;

            for (int i = currentIndex; i < Steps.Count; i++)
            {
                var step = Steps[i];
                if (!step.WaitingForStartDelay)
                    continue;

                if (step.StartDelayRemaining > 0f)
                {
                    step.StartDelayRemaining -= deltaTime;
                    if (step.StartDelayRemaining <= 0f)
                    {
                        step.StartDelayRemaining = 0f;
                        anyElapsed = true;
                    }
                }

                if (step.blocking && step.StartDelayRemaining > 0f)
                {
                    break;
                }
            }

            return anyElapsed;
        }

        private bool ShouldPauseTimeLimits()
        {
            if (parallelRunning.Count > 0)
            {
                return false;
            }

            if (currentIndex >= Steps.Count)
            {
                return false;
            }

            var step = Steps[currentIndex];
            return !step.Started && step.WaitingForStartDelay && step.StartDelayRemaining > 0f;
        }

        private void StartNextEligibleSteps()
        {
            while (currentIndex < Steps.Count)
            {
                var step = Steps[currentIndex];

                if (!step.Started)
                {
                    if (!step.WaitingForStartDelay)
                    {
                        step.StartDelayRemaining = Mathf.Max(0f, step.StartDelayRemaining);
                        if (step.StartDelayRemaining > 0f)
                        {
                            step.WaitingForStartDelay = true;
                            return;
                        }
                    }
                    else if (step.StartDelayRemaining > 0f)
                    {
                        return;
                    }

                    step.WaitingForStartDelay = false;
                    StartStep(step);
                }

                if (step.blocking)
                {
                    // Wait for its completion/failure event
                    return;
                }

                // Non-blocking: advance immediately and keep starting subsequent non-blocking steps
                currentIndex++;
            }
        }

        private void StartStep(Step s)
        {
            if (s.Action == null) return;

            s.Started = true;
            s.TimeLeft = (s.timeLimited && s.timeLimit > 0f) ? s.timeLimit : float.PositiveInfinity;
            s.suppressLoopOnce = false; // new run ⇒ clear suppression
            s.WaitingForStartDelay = false;
            s.StartDelayRemaining = 0f;

            AttachStepListeners(s);      // attach before Begin to catch instant-complete
            SafeBegin(s.Action);

            if (!parallelRunning.Contains(s))
                parallelRunning.Add(s);
        }

        private void RestartStep(Step s)
        {
            // Clean slate: cancel & detach, then re-attach and start
            DetachStepListeners(s);
            SafeCancel(s.Action);
            parallelRunning.Remove(s);
            s.Started = false;

            StartStep(s);
        }

        private void HandleStepTerminal(Step s)
        {
            DetachStepListeners(s);
            parallelRunning.Remove(s);

            if (s.blocking)
            {
                // Advance the sequence index past the blocking step (s should be the current one)
                if (currentIndex < Steps.Count && ReferenceEquals(Steps[currentIndex], s))
                    currentIndex++;

                StartNextEligibleSteps();
            }

            MaybeRestartOrComplete();
        }

        /// <summary>
        /// Complete the sequence when all steps are finished.
        /// </summary>
        private void MaybeRestartOrComplete()
        {
            if (currentIndex < Steps.Count || parallelRunning.Count > 0)
                return; // still running steps

            // All steps finished - sequence runs only once
            Complete();
        }

        // ---------- event wiring ----------

        private void AttachStepListeners(Step s)
        {
            if (s.Action == null) return;

            // Avoid multiple subscriptions if restarted
            DetachStepListeners(s);

            s.completedHandler = () =>
            {
                if (!running) return;

                if (ShouldRestartStep(s))
                {
                    RestartStep(s);
                }
                else
                {
                    // If we suppressed a loop due to timeout, clear it now.
                    s.suppressLoopOnce = false;
                    HandleStepTerminal(s);
                }
            };

            s.failedHandler = () =>
            {
                if (!running) return;

                // Treat failure as terminal for progression.
                HandleStepTerminal(s);
            };

            s.Action.ProcessCompleted += s.completedHandler;
            s.Action.ProcessFailed += s.failedHandler;
        }

        private void DetachStepListeners(Step s)
        {
            if (s?.Action == null) return;

            if (s.completedHandler != null)
            {
                s.Action.ProcessCompleted -= s.completedHandler;
                s.completedHandler = null;
            }

            if (s.failedHandler != null)
            {
                s.Action.ProcessFailed -= s.failedHandler;
                s.failedHandler = null;
            }
        }

        private void ResetAllForRepeat()
        {
            foreach (var s in Steps)
            {
                DetachStepListeners(s);
                SafeCancel(s.Action);
                ResetStepRuntimeState(s, true);
            }

            parallelRunning.Clear();
            currentIndex = 0;

            // sequenceTimeLeft is preserved in TimeLimited mode (counting down in coroutine)
            // and left as +inf in Infinite mode.
        }

        private void InitializeStepRuntime()
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                ResetStepRuntimeState(Steps[i], true);
            }
        }

        private void ResetStepRuntimeState(Step step, bool resetLoopCount)
        {
            step.Started = false;
            step.suppressLoopOnce = false;
            step.WaitingForStartDelay = false;
            step.StartDelayRemaining = Mathf.Max(0f, step.startDelay);
            step.TimeLeft = (step.timeLimited && step.timeLimit > 0f) ? step.timeLimit : float.PositiveInfinity;

            if (resetLoopCount)
            {
                step.loopsCompleted = 0;
            }
        }

        private bool ShouldRestartStep(Step step)
        {
            if (step.suppressLoopOnce)
            {
                step.suppressLoopOnce = false;
                return false;
            }

            switch (step.loopMode)
            {
                case StepLoopMode.Once:
                    return false;

                case StepLoopMode.Infinite:
                    return true;

                case StepLoopMode.Limited:
                    step.loopsCompleted++;
                    return step.loopsCompleted < step.loopCount;

                default:
                    return false;
            }
        }

        // --- safe wrappers (adapt if your ProcessAction API differs) ---

        private static void SafeBegin(ProcessAction a)
        {
        
            a.Begin();
        }

        private static void SafeCancel(ProcessAction a)
        {
          
            if (a.IsInProgress) a.Cancel();
        }

        private static void SafeForceComplete(ProcessAction a)
        {
          
            if (!a.IsComplete) a.Complete();
        }
    }
}
