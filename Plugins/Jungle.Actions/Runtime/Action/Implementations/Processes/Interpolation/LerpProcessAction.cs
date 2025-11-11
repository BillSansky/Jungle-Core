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
    /// Implements the interrupt behavior action.
    /// </summary>
    public enum InterruptBehavior
    {
        StayAtCurrent,
        GoToStart,
        GoToEnd
    }
    /// <summary>
    /// Implements the lerp process action action.
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
        private Action completionCallback;

        public event Action OnProcessCompleted;

        /// <summary>
        /// Indicates whether the action can report a finite duration.
        /// </summary>

        public bool HasDefinedDuration => duration?.V > 0f;
        /// <summary>
        /// Gets the total duration of the action in seconds.
        /// </summary>
        public float Duration => duration?.V ?? 0f;
        /// <summary>
        /// Gets whether the action is currently running.
        /// </summary>
        public bool IsInProgress => isInProgress;
        /// <summary>
        /// Gets whether the action has finished executing.
        /// </summary>
        public bool HasCompleted => hasCompleted;
        /// <summary>
        /// Executes the lerp immediately by delegating to <see cref="Start"/>.
        /// </summary>

        public void Execute()
        {
            Start(null);
        }

        /// <summary>
        /// Starts the lerp process action.
        /// </summary>
        /// <param name="callback"></param>
        public void Start(Action callback = null)
        {
            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;
            currentIteration = 0;
            completionCallback = callback;

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
        /// Stops the lerp process action before completion.
        /// </summary>

        public void Stop()
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
            completionCallback = null;

            OnInterrupted();
        }

        protected abstract T GetStartValue();
        protected abstract T GetEndValue();
        protected abstract T LerpValue(T start, T end, float t);
        protected abstract void ApplyValue(T value);

        protected virtual void OnBeforeStart() { }
        protected virtual void OnIterationCompleted() { }
        protected virtual void OnInterrupted() { }

        private void Complete()
        {
            if (!isInProgress)
            {
                return;
            }

            isInProgress = false;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            completionCallback?.Invoke();
            completionCallback = null;
        }

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
