using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values.Primitives;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Enumerates the InterruptBehavior values.
    /// </summary>
    public enum InterruptBehavior
    {
        StayAtCurrent,
        GoToStart,
        GoToEnd
    }
    /// <summary>
    /// Base class that provides interpolation helpers and timing for lerp-based actions.
    /// </summary>
    [Serializable]
    public abstract class LerpProcessAction<T> : IProcessAction
    {
        [SerializeReference][JungleClassSelection] protected IFloatValue duration = new FloatValue(0.35f);
        [SerializeReference][JungleClassSelection] protected IAnimationCurveValue interpolation =
            new AnimationCurveValue(AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
        [SerializeReference][JungleClassSelection] protected ILoopStrategy loopStrategy = new OnceLoopStrategy();
        [SerializeField] protected InterruptBehavior interruptBehavior = InterruptBehavior.StayAtCurrent;

        private Coroutine activeRoutine;
        private bool isInProgress;
        private bool hasCompleted;
        private int currentIteration;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => duration?.V > 0f;
        public float Duration => duration?.V ?? 0f;
        public bool IsInProgress => isInProgress;
        public bool HasCompleted => hasCompleted;
        /// <summary>
        /// Triggers the action as part of a process by delegating to <see cref="Start"/>.
        /// </summary>
        public void Execute()
        {
            Start();
        }
        /// <summary>
        /// Initializes interpolation state and begins lerping, falling back to the end value if the duration is zero.
        /// </summary>
        public void Start()
        {
            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;
            currentIteration = 0;

            loopStrategy?.Reset();

            var totalDuration = duration.V;
            var curve = interpolation.V;

            OnBeforeStart();

            StopActiveRoutine();

            if (totalDuration <= 0f)
            {
                var endValue = GetEndValue();
                ApplyValue(endValue);
                Complete();
                return;
            }

            activeRoutine = CoroutineRunner.StartManagedCoroutine(LerpCoroutine(totalDuration, curve));
        }
        /// <summary>
        /// Stops the active interpolation and snaps to the configured interrupt position.
        /// </summary>
        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            StopActiveRoutine();

            switch (interruptBehavior)
            {
                case InterruptBehavior.GoToStart:
                    ApplyValue(GetStartValue());
                    break;
                case InterruptBehavior.GoToEnd:
                    ApplyValue(GetEndValue());
                    break;
                case InterruptBehavior.StayAtCurrent:
                default:
                    // Do nothing, stay at current value
                    break;
            }

            isInProgress = false;
            hasCompleted = false;
            currentIteration = 0;

            OnInterrupted();
        }
        /// <summary>
        /// Returns the value that should be applied at the start of the interpolation.
        /// </summary>
        protected abstract T GetStartValue();
        /// <summary>
        /// Returns the value the interpolation should reach when it finishes.
        /// </summary>
        protected abstract T GetEndValue();
        /// <summary>
        /// Calculates an interpolated value between the start and end values for a given progress ratio.
        /// </summary>
        protected abstract T LerpValue(T start, T end, float t);
        /// <summary>
        /// Pushes the supplied value to the concrete destination controlled by the action.
        /// </summary>
        protected abstract void ApplyValue(T value);
        /// <summary>
        /// Handles the OnBeforeStart event.
        /// </summary>
        protected virtual void OnBeforeStart() { }
        /// <summary>
        /// Handles the OnIterationCompleted event.
        /// </summary>
        protected virtual void OnIterationCompleted() { }
        /// <summary>
        /// Handles the OnInterrupted event.
        /// </summary>
        protected virtual void OnInterrupted() { }
        /// <summary>
        /// Marks the process as finished and notifies listeners once the interpolation ends.
        /// </summary>
        private void Complete()
        {
            if (!isInProgress)
            {
                return;
            }

            isInProgress = false;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
        }
        /// <summary>
        /// Drives the interpolation coroutine, looping according to the configured strategy until completion.
        /// </summary>
        private IEnumerator LerpCoroutine(float totalDuration, AnimationCurve curve)
        {
            Debug.Assert(loopStrategy != null, "Loop strategy is null.");

            var durationClamp = Mathf.Max(0.0001f, totalDuration);

            do
            {

                float time = 0f;

                while (time < durationClamp)
                {
                    time += Time.deltaTime;
                    var normalizedTime = time / durationClamp;
                    var loopAdjustedTime = loopStrategy.GetCurrentTime(normalizedTime, currentIteration);
                    var curved = curve.Evaluate(loopAdjustedTime);
                    ApplyValue(LerpValue(GetStartValue(),  GetEndValue(), curved));
                    yield return null;
                }

                ApplyValue( GetEndValue());
                currentIteration++;
                OnIterationCompleted();

            } while (loopStrategy.ShouldContinue(currentIteration));

            Complete();
        }
        /// <summary>
        /// Cancels the currently running interpolation coroutine, if any.
        /// </summary>
        private void StopActiveRoutine()
        {
            if (activeRoutine == null)
            {
                return;
            }

            CoroutineRunner.StopManagedCoroutine(activeRoutine);
            activeRoutine = null;
        }
    }
}
