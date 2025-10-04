using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Event-driven sequence of child ProcessActions.
    /// - Progression listens to child ProcessCompleted/ProcessFailed (no polling).
    /// - Coroutine only services time limits (sequence & per-step).
    /// - In Loop mode: restarts when all steps finish.
    /// - In TimeLimited mode: restarts when all steps finish while time remains; completes/cancels only when time expires.
    /// - For per-step timeout with finishExecutionOnEndTime=true: do NOT force-complete; instead suppress exactly one loop on next natural completion.
    /// </summary>
    [Serializable]
    public class ActionSequence : ProcessAction
    {
        [SerializeField]
        public List<Step> Steps = new List<Step>();

        public enum ProcessMode
        {
            TimeLimited, // Repeats the whole sequence until SequenceTimeLimit elapses.
            Loop,        // Repeats the whole sequence indefinitely until externally completed/canceled.
            Once         // Runs once through all steps.
        }

        [Header("Sequence Settings")]
        public ProcessMode Mode = ProcessMode.Once;

        [Tooltip("Delay in seconds before the first step starts running when the sequence begins.")]
        public float StartDelay = 0f;

        [Tooltip("Only used when Mode == TimeLimited.")]
        public float SequenceTimeLimit = 0f;

        [Tooltip("When the sequence time expires (Mode == TimeLimited), mark the sequence as complete instead of canceled.")]
        public bool FinishOnSequenceTimeLimit = true;

        /// <summary>True if the sequence itself is time-limited or any step has a time limit.</summary>
        public override bool IsTimed =>
            (Mode == ProcessMode.TimeLimited && SequenceTimeLimit > 0f) ||
            (Steps != null && Steps.Exists(s =>
                (s.timeLimited && s.timeLimit > 0f) ||
                (s.overrideSequenceMode && s.mode == ProcessMode.TimeLimited && s.modeTimeLimit > 0f)));

        public override float Duration
        {
            get
            {
                if (Mode == ProcessMode.TimeLimited)
                {
                    return SequenceTimeLimit;
                }

                if (Mode == ProcessMode.Loop)
                {
                    return 0f; // Infinite loop has no specific duration
                }

                // Mode == Once: calculate total duration of all steps
                float totalDuration = StartDelay;
                if (Steps == null || Steps.Count == 0)
                    return totalDuration;

                foreach (var step in Steps)
                {
                    totalDuration += step.startDelay;
                    if (step.Action != null && step.Action.IsTimed)
                    {
                        totalDuration += step.Action.Duration;
                    }
                }

                return totalDuration;
            }
        }

        [Serializable]
        public class Step
        {
            [JungleClassSelection] [SerializeReference]
            public ProcessAction Action;

            [Header("Mode Override")]
            [Tooltip("If true, this step can override the sequence mode.")]
            public bool overrideSequenceMode;

            [Tooltip("Mode that will be used when overrideSequenceMode is true.")]
            public ProcessMode mode = ProcessMode.Once;

            [Tooltip("If true, when the action completes it restarts until the entire sequence ends.")]
            public bool loopTillEnd;

            [Tooltip("When looping, limits how many times this step can run. Zero or negative means unlimited loops.")]
            [Min(0)]
            public int loopCount = 0;

            [Tooltip("If true, the next step waits for this one to finish (blocking). If false, the next step starts immediately (parallel).")]
            public bool blocking = true;

            [Header("Step Start Delay")]
            [Tooltip("Delay in seconds before this step begins after it becomes eligible.")]
            public float startDelay = 0f;

            [Header("Mode Time Limit")]
            [Tooltip("Only used when overriding the mode to TimeLimited. Total seconds this step is allowed to run across all loops.")]
            public float modeTimeLimit = 0f;

            [Tooltip("Only used when overriding the mode to TimeLimited. If true the step is forced complete when the mode time expires; otherwise it is canceled.")]
            public bool finishOnModeTimeLimit = true;

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

            [NonSerialized] internal bool ModeTimeActive;
            [NonSerialized] internal float ModeTimeLeft;

            // Event handlers to detach safely
            [NonSerialized] internal Action completedHandler;
            [NonSerialized] internal Action failedHandler;
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

            sequenceTimeLeft = (Mode == ProcessMode.TimeLimited && SequenceTimeLimit > 0f)
                ? SequenceTimeLimit
                : (Mode == ProcessMode.Loop ? float.PositiveInfinity : float.PositiveInfinity);

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

            // Sequence time limiting
            if (Mode == ProcessMode.TimeLimited && SequenceTimeLimit > 0f)
            {
                sequenceTimeLeft -= deltaTime;
                if (sequenceTimeLeft <= 0f)
                {
                    if (FinishOnSequenceTimeLimit) Complete();
                    else Cancel();
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

                    if (step.loopTillEnd)
                    {
                        RestartStep(step);
                    }
                    else
                    {
                        HandleStepTerminal(step);
                    }
                }

                // Reset the per-step timer so we don't keep firing every frame.
                // If we want "single-shot" timeout per run, set to +inf; otherwise re-arm.
                step.TimeLeft = float.PositiveInfinity;
            }

            for (int i = 0; i < Steps.Count; i++)
            {
                var step = Steps[i];
                if (!step.overrideSequenceMode || step.mode != ProcessMode.TimeLimited)
                    continue;

                if (!step.ModeTimeActive || step.modeTimeLimit <= 0f)
                    continue;

                step.ModeTimeLeft -= deltaTime;
                if (step.ModeTimeLeft > 0f)
                    continue;

                step.ModeTimeLeft = 0f;
                step.ModeTimeActive = false;

                if (step.Action == null)
                    continue;

                if (step.finishOnModeTimeLimit)
                    SafeForceComplete(step.Action);
                else
                    SafeCancel(step.Action);
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
            if (s.overrideSequenceMode && s.mode == ProcessMode.TimeLimited)
            {
                s.ModeTimeActive = s.modeTimeLimit > 0f && s.ModeTimeLeft > 0f;
            }
            else
            {
                s.ModeTimeActive = false;
            }

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
            s.ModeTimeActive = false;

            StartStep(s);
        }

        private void HandleStepTerminal(Step s)
        {
            DetachStepListeners(s);
            parallelRunning.Remove(s);
            s.ModeTimeActive = false;

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
        /// Decide whether to restart the whole sequence or complete it,
        /// based on mode and remaining time.
        /// </summary>
        private void MaybeRestartOrComplete()
        {
            if (currentIndex < Steps.Count || parallelRunning.Count > 0)
                return; // still running steps

            // All steps finished
            if (Mode == ProcessMode.Loop)
            {
                ResetAllForRepeat();
                StartNextEligibleSteps();
                return;
            }

            if (Mode == ProcessMode.TimeLimited)
            {
                // If time remains, immediately start the sequence again.
                if (sequenceTimeLeft > 0f)
                {
                    ResetAllForRepeat();
                    StartNextEligibleSteps();
                    return;
                }

                // No time left – end handled by ServiceTimeLimits, but be safe:
                if (FinishOnSequenceTimeLimit) Complete();
                else Cancel();
                return;
            }

            // Once
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
                s.ModeTimeActive = false;
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

            if (Steps.Count > 0)
            {
                Steps[0].StartDelayRemaining += Mathf.Max(0f, StartDelay);
            }

            // sequenceTimeLeft is preserved in TimeLimited mode (counting down in coroutine)
            // and left as +inf in Loop mode.
        }

        private void InitializeStepRuntime()
        {
            for (int i = 0; i < Steps.Count; i++)
            {
                ResetStepRuntimeState(Steps[i], true);
            }

            if (Steps.Count > 0)
            {
                Steps[0].StartDelayRemaining += Mathf.Max(0f, StartDelay);
            }
        }

        private ProcessMode GetEffectiveMode(Step step)
        {
            return step.overrideSequenceMode ? step.mode : Mode;
        }

        private void ResetStepRuntimeState(Step step, bool resetModeState)
        {
            step.Started = false;
            step.suppressLoopOnce = false;
            step.WaitingForStartDelay = false;
            step.StartDelayRemaining = Mathf.Max(0f, step.startDelay);
            step.TimeLeft = (step.timeLimited && step.timeLimit > 0f) ? step.timeLimit : float.PositiveInfinity;

            if (resetModeState)
            {
                step.loopsCompleted = 0;
                var effectiveMode = GetEffectiveMode(step);
                if (step.overrideSequenceMode && effectiveMode == ProcessMode.TimeLimited)
                {
                    step.ModeTimeLeft = Mathf.Max(0f, step.modeTimeLimit);
                }
                else
                {
                    step.ModeTimeLeft = 0f;
                }
            }

            step.ModeTimeActive = false;
        }

        private bool ShouldRestartStep(Step step)
        {
            if (step.suppressLoopOnce)
            {
                step.suppressLoopOnce = false;
                return false;
            }

            var effectiveMode = GetEffectiveMode(step);
            var loopsAllowed = false;

            if (step.overrideSequenceMode)
            {
                switch (effectiveMode)
                {
                    case ProcessMode.Loop:
                        loopsAllowed = true;
                        break;
                    case ProcessMode.TimeLimited:
                        loopsAllowed = step.modeTimeLimit > 0f && step.ModeTimeLeft > 0f;
                        break;
                }
            }
            else
            {
                if (!step.loopTillEnd)
                {
                    return false;
                }

                if (effectiveMode == ProcessMode.Loop || effectiveMode == ProcessMode.TimeLimited)
                {
                    loopsAllowed = true;
                }
            }

            if (!loopsAllowed)
            {
                return false;
            }

            if (step.loopCount > 0)
            {
                step.loopsCompleted++;
                if (step.loopsCompleted >= step.loopCount)
                {
                    return false;
                }
            }

            return true;
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
