using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Jungle.Attributes;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Runs a configurable list of <see cref="IProcessAction"/> steps and handles delays, looping, and time limits.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Sequence Action", "Runs a configurable sequence of process steps with delays and time limits.", null, "Actions/Process")]
    public class SequenceAction : IProcessAction
    {
        /// <summary>
        /// Defines the ordered steps executed by the sequence.
        /// </summary>
        [SerializeField] public List<Step> Steps = new List<Step>();

        [Header("Sequence Settings")] [Tooltip("Enable to set a time limit for the entire sequence.")]
        public bool hasSequenceTimeLimit = false;
        /// <summary>
        /// Maximum allowed duration for the entire sequence in seconds.
        /// </summary>

        [Tooltip("Time limit for the entire sequence in seconds.")]
        public float SequenceTimeLimit = 0f;


        /// <summary>
        /// Indicates whether all sequence steps have finished.
        /// </summary>
        public bool HasCompleted { get; private set; }

        /// <summary>True if the sequence itself is time-limited or any step has a time limit.</summary>
        public bool HasDefinedDuration =>
            (hasSequenceTimeLimit) || Steps.All(s => s.Action.HasDefinedDuration || s.timeLimited);

        /// <inheritdoc />
        public float Duration
        {
            get
            {
                // If sequence has a time limit, return the shorter of sequence time limit or total step duration
                float totalDuration = 0f;
                if (Steps.Count > 0)
                {
                    foreach (var step in Steps)
                    {
                        totalDuration += step.startDelay;
                        float? duration = step.GetDuration();
                        if (!duration.HasValue)
                            return -1;

                        totalDuration += duration.Value;
                    }
                }

                if (hasSequenceTimeLimit && SequenceTimeLimit > 0f)
                {
                    return totalDuration > 0f ? Mathf.Min(totalDuration, SequenceTimeLimit) : SequenceTimeLimit;
                }

                return totalDuration;
            }
        }

        /// <inheritdoc />
        public bool IsInProgress { get; private set; }
        /// <summary>
        /// Describes how a step repeats when executed.
        /// </summary>

        public enum StepLoopMode
        {
            Once, // Start only once
            Infinite, // Infinite indefinitely
            Limited // Infinite a specific number of times
        }
        /// <summary>
        /// Represents a single step in the sequence.
        /// </summary>

        [Serializable]
        public class Step
        {
            /// <summary>
            /// Process action executed for this step.
            /// </summary>
            [JungleClassSelection] [SerializeReference]
            public IProcessAction Action;

            [Header("Infinite Settings")]
            [Tooltip(
                "Controls how this step loops: Once (no loop), Infinite (infinite), or Limited (specific number of loops).")]
            /// <summary>
            /// Determines how this step repeats when executed.
            /// </summary>
            public StepLoopMode loopMode = StepLoopMode.Once;
            /// <summary>
            /// Number of loops to execute when <see cref="loopMode"/> is <see cref="StepLoopMode.Limited"/>.
            /// </summary>

            [Tooltip("Number of times to execute this step when loopMode is Limited.")] [Min(1)]
            public int loopCount = 1;

            [Tooltip(
                "If true, the next step waits for this one to finish (blocking). If false, the next step starts immediately (parallel).")]
            /// <summary>
            /// When true, the next step waits until this one completes.
            /// </summary>
            public bool blocking = true;
            /// <summary>
            /// Delay applied before this step begins once eligible.
            /// </summary>

            [Header("Step Start Delay")]
            [Tooltip("Delay in seconds before this step begins after it becomes eligible.")]
            public float startDelay = 0f;

            [Header("Step Time Limit")] public bool timeLimited;

            [Tooltip(
                "If true, when the time limit elapses we do NOT force-complete; we let it finish naturally and skip exactly one loop on completion. If false, we cancel on timeout.")]
            /// <summary>
            /// When true, allows the action to finish naturally after the time limit elapses.
            /// </summary>
            public bool finishExecutionOnEndTime;
            /// <summary>
            /// Seconds permitted for this step when <see cref="timeLimited"/> is true.
            /// </summary>

            [Tooltip("Seconds allowed for this step if timeLimited is true.")]
            public float timeLimit = 0f;

            // Runtime (not serialized)
            [NonSerialized] internal float timeLeft;
            [NonSerialized] internal bool started;

            [NonSerialized] internal bool waitingForStartDelay;
            [NonSerialized] internal float startDelayRemaining;

            // Timeout behavior: after a timeout (finishExecutionOnEndTime == true),
            // allow the action to complete naturally, but skip exactly one loop on that completion.
            [NonSerialized] internal bool suppressLoopOnce;

            [NonSerialized] internal int loopsCompleted;

            // Event handlers to detach safely
            [NonSerialized] internal Action completedHandler;


            /// <summary>
            /// Gets the duration of this step based on time limit or action duration.
            /// Returns the shorter of the two if both are available.
            /// Returns null if no duration can be determined.
            /// </summary>
            public float? GetDuration()
            {
                float? actionDuration = null;
                float? stepTimeLimit = null;

                // GetContext action duration if available
                if (Action != null && Action.HasDefinedDuration && Action.Duration > 0f)
                {
                    actionDuration = Action.Duration;
                }

                // GetContext time limit if set
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


        [NonSerialized] private int currentIndex;
        [NonSerialized] private float sequenceTimeLeft;

        [NonSerialized] private readonly List<Step> parallelRunning = new List<Step>();

        // Coroutine handle
        [NonSerialized] private Coroutine tickRoutine;

        /// <param name="callback"></param>
        /// <inheritdoc />
        public void Start(Action callback)
        {
            IsInProgress = true;
            HasCompleted = false;

            if (Steps.Count == 0)
            {
                Complete();
                return;
            }

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

        /// <inheritdoc />
        public void Interrupt()
        {
            if (!IsInProgress) return;

            IsInProgress = false;
            InterruptOrCompleteCleanup();
        }

        /// <summary>
        /// Interrupts the sequence when its owning state is exited.
        /// </summary>
        public void OnStateExit()
        {
            if (IsInProgress)
            {
                Interrupt();
            }

            InterruptOrCompleteCleanup();
            HasCompleted = false;
        }

        private void Complete()
        {
            if (!IsInProgress) return;

            IsInProgress = false;
            HasCompleted = true;
            InterruptOrCompleteCleanup();
            OnProcessCompleted?.Invoke();
        }

        protected void InterruptOrCompleteCleanup()
        {
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
            while (IsInProgress)
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
            if (!IsInProgress) return;

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

                step.timeLeft -= deltaTime;
                if (step.timeLeft > 0f) continue;

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
                    step.Action.Interrupt();
                    HandleStepTerminal(step);
                }

                // Reset the per-step timer so we don't keep firing every frame.
                // If we want "single-shot" timeout per run, set to +inf; otherwise re-arm.
                step.timeLeft = float.PositiveInfinity;
            }
        }

        // ---------- progression helpers (event-driven) ----------

        private bool UpdateStartDelays(float deltaTime)
        {
            var anyElapsed = false;

            for (int i = currentIndex; i < Steps.Count; i++)
            {
                var step = Steps[i];
                if (!step.waitingForStartDelay)
                    continue;

                if (step.startDelayRemaining > 0f)
                {
                    step.startDelayRemaining -= deltaTime;
                    if (step.startDelayRemaining <= 0f)
                    {
                        step.startDelayRemaining = 0f;
                        anyElapsed = true;
                    }
                }

                if (step.blocking && step.startDelayRemaining > 0f)
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
            return !step.started && step.waitingForStartDelay && step.startDelayRemaining > 0f;
        }

        private void StartNextEligibleSteps()
        {
            while (currentIndex < Steps.Count)
            {
                var step = Steps[currentIndex];

                if (!step.started)
                {
                    if (!step.waitingForStartDelay)
                    {
                        step.startDelayRemaining = Mathf.Max(0f, step.startDelayRemaining);
                        if (step.startDelayRemaining > 0f)
                        {
                            step.waitingForStartDelay = true;
                            return;
                        }
                    }
                    else if (step.startDelayRemaining > 0f)
                    {
                        return;
                    }

                    step.waitingForStartDelay = false;
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

            s.started = true;
            s.timeLeft = (s.timeLimited && s.timeLimit > 0f) ? s.timeLimit : float.PositiveInfinity;
            s.suppressLoopOnce = false; // new run ⇒ clear suppression
            s.waitingForStartDelay = false;
            s.startDelayRemaining = 0f;

            AttachStepListeners(s); // attach before Start to catch instant-complete
            s.Action.Start(null);

            if (!parallelRunning.Contains(s))
                parallelRunning.Add(s);
        }

        private void RestartStep(Step s)
        {
            // Clean slate: cancel & detach, then re-attach and start
            DetachStepListeners(s);
            s.Action.Interrupt();
            parallelRunning.Remove(s);
            s.started = false;

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
        /// OnStateExit the sequence when all steps are finished.
        /// </summary>
        private void MaybeRestartOrComplete()
        {
            if (currentIndex < Steps.Count || parallelRunning.Count > 0)
                return; // still running steps
            
            
            
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
                if (!IsInProgress) return;

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
        }

        private void DetachStepListeners(Step s)
        {
            if (s?.Action == null) return;

            if (s.completedHandler != null)
            {
                s.completedHandler = null;
            }
        }

        private void ResetAllForRepeat()
        {
            foreach (var s in Steps)
            {
                DetachStepListeners(s);
                s.Action.Interrupt();
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
            step.started = false;
            step.suppressLoopOnce = false;
            step.waitingForStartDelay = false;
            step.startDelayRemaining = Mathf.Max(0f, step.startDelay);
            step.timeLeft = (step.timeLimited && step.timeLimit > 0f) ? step.timeLimit : float.PositiveInfinity;

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
    }
}