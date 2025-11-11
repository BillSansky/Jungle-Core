using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Runs a collection of <see cref="IImmediateAction"/> tasks repeatedly for a configurable duration or indefinitely.
    /// </summary>
    [Serializable]
    [JungleClassInfo(
        "Immediate Task Process Action",
        "Runs immediate tasks for a fixed duration or until stopped.",
        null,
        "Actions/Process")]
    public class ImmediateTaskProcessAction : IProcessAction
    {
        [SerializeReference]
        [JungleClassSelection(typeof(IImmediateAction))]
        public List<IImmediateAction> tasks = new List<IImmediateAction>();

        [Tooltip("When true, the tasks continue executing until Stop() is called.")]
        public bool runIndefinitely;

        [SerializeReference]
        [JungleClassSelection(typeof(IFloatValue))]
        [Tooltip("Duration to run the tasks when not running indefinitely.")]
        public IFloatValue duration = new FloatValue(1f);

        [Tooltip("Optional interval in seconds between task executions. Zero executes every frame.")]
        [SerializeReference]
        [JungleClassSelection(typeof(IFloatValue))]
        public IFloatValue interval = new FloatValue(0f);

        [Tooltip("When true uses unscaled time for duration and interval tracking.")]
        public bool useUnscaledTime;

        private Coroutine activeRoutine;
        private bool isInProgress;
        private Action completionCallback;
        private float intervalAccumulator;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => !runIndefinitely;

        public float Duration => runIndefinitely ? -1f : Mathf.Max(0f, GetDurationValue());

        public bool IsInProgress => isInProgress;

        public void StartProcess(Action callback = null)
        {
            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            completionCallback = callback;
            intervalAccumulator = 0f;

            var durationValue = GetDurationValue();

            if (!runIndefinitely && durationValue <= 0f)
            {
                ExecuteTasks();
                Complete();
                return;
            }

            activeRoutine = runIndefinitely
                ? CoroutineRunner.StartManagedCoroutine(RunIndefiniteRoutine())
                : CoroutineRunner.StartManagedCoroutine(RunForDurationRoutine(durationValue));
        }

        public void Stop()
        {
            if (!isInProgress)
            {
                return;
            }

            StopRoutine();

            isInProgress = false;
            completionCallback = null;
        }

        private IEnumerator RunForDurationRoutine(float durationSeconds)
        {
            var elapsed = 0f;
            var durationClamp = Mathf.Max(0f, durationSeconds);

            while (elapsed < durationClamp && isInProgress)
            {
                var deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                elapsed += deltaTime;
                Tick(deltaTime);
                yield return null;
            }

            activeRoutine = null;
            Complete();
        }

        private IEnumerator RunIndefiniteRoutine()
        {
            while (isInProgress)
            {
                var deltaTime = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                Tick(deltaTime);
                yield return null;
            }
        }

        private void Tick(float deltaTime)
        {
            var intervalValue = GetIntervalValue();

            if (intervalValue <= 0f)
            {
                ExecuteTasks();
                return;
            }

            intervalAccumulator += deltaTime;
            if (intervalAccumulator >= intervalValue)
            {
                intervalAccumulator = 0f;
                ExecuteTasks();
            }
        }

        private void ExecuteTasks()
        {
            for (var index = 0; index < tasks.Count; index++)
            {
                var task = tasks[index];
                if (task == null)
                {
                    Debug.LogWarning($"Immediate task process action has a null task at index {index}.");
                    continue;
                }

                task.Execute();
            }
        }

        private void Complete()
        {
            if (!isInProgress)
            {
                return;
            }

            StopRoutine();

            isInProgress = false;
            OnProcessCompleted?.Invoke();
            completionCallback?.Invoke();
            completionCallback = null;
        }

        private void StopRoutine()
        {
            if (activeRoutine == null)
            {
                return;
            }

            CoroutineRunner.StopManagedCoroutine(activeRoutine);
            activeRoutine = null;
        }

        private float GetDurationValue()
        {
            return duration?.V ?? 0f;
        }

        private float GetIntervalValue()
        {
            return interval?.V ?? 0f;
        }
    }
}
