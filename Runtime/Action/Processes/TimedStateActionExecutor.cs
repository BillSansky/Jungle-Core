using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Specifies how the executor determines when to finish.
    /// </summary>
    public enum EndLogic
    {
        Timed,
        OneFrame,
    }
    /// <summary>
    /// Runs state actions for a timed or single-frame duration.
    /// </summary>
    
    [Serializable]
    [JungleClassInfo("Timed State Action Executor", "Runs state actions for a timed or single-frame duration.", null, "Actions/Process")]
    public class TimedStateActionExecutor : IProcessAction
    {
        /// <summary>
        /// Defines the immediate actions executed during the process.
        /// </summary>

        [SerializeReference][JungleClassSelection]
        public List<IImmediateAction> Actions  = new List<IImmediateAction>();
        /// <summary>
        /// Controls how the executor determines when to finish.
        /// </summary>

        public EndLogic endLogic = EndLogic.Timed;
        /// <summary>
        /// Duration used when <see cref="EndLogic.Timed"/> is selected.
        /// </summary>

        public float duration = 1.0f;
        /// <summary>
        /// MonoBehaviour instance used to run timing coroutines.
        /// </summary>

        [NonSerialized]
        public MonoBehaviour coroutineRunner;

        private Coroutine autoEndCoroutine;
        private bool isInProgress;
        private bool hasCompleted;
        private Action completionCallback;

        public event Action OnProcessCompleted;

        /// <summary>
        /// Indicates whether the action can report a finite duration.
        /// </summary>

        public bool HasDefinedDuration => true;
        /// <summary>
        /// Gets the total duration of the action in seconds.
        /// </summary>

        public float Duration
        {
            get
            {
                switch (endLogic)
                {
                    case EndLogic.Timed:
                        return duration;
                    case EndLogic.OneFrame:
                        return 0.0f;
                    default:
                        return 0.0f;
                }
            }
        }
        /// <summary>
        /// Gets whether the action is currently running.
        /// </summary>

        public bool IsInProgress => isInProgress;
        /// <summary>
        /// Gets whether the action has finished executing.
        /// </summary>

        public bool HasCompleted => hasCompleted;
        /// <summary>
        /// Executes the process immediately by delegating to <see cref="StartProcess"/>.
        /// </summary>

        public void Execute()
        {
            StartProcess(null);
        }

        /// <summary>
        /// Starts the timed state action executor.
        /// </summary>
        /// <param name="callback"></param>
        public void StartProcess(Action callback = null)
        {
            Debug.Assert(coroutineRunner != null, "coroutineRunner must be set before starting the action");

            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;
            completionCallback = callback;

            foreach (var action in Actions)
            {
                action?.StartProcess();
            }

            // StartProcess auto-end coroutine if needed
            if ((endLogic == EndLogic.Timed || endLogic == EndLogic.OneFrame) && coroutineRunner != null)
            {
                autoEndCoroutine = coroutineRunner.StartCoroutine(AutoEndCoroutine());
            }
        }
        /// <summary>
        /// Stops the timed state action executor before completion.
        /// </summary>

        public void Stop()
        {
            if (!isInProgress)
            {
                return;
            }

            Cleanup();
            isInProgress = false;
            hasCompleted = false;
            completionCallback = null;
        }

        private void Cleanup()
        {
            // Stop the auto-end coroutine if it's running
            if (autoEndCoroutine != null && coroutineRunner != null)
            {
                coroutineRunner.StopCoroutine(autoEndCoroutine);
                autoEndCoroutine = null;
            }

            foreach (var action in Actions)
            {
                action?.Stop();
            }
        }

        private void Complete()
        {
            if (!isInProgress)
            {
                return;
            }

            Cleanup();
            isInProgress = false;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            completionCallback?.Invoke();
            completionCallback = null;
        }

        private IEnumerator AutoEndCoroutine()
        {
            if (endLogic == EndLogic.OneFrame)
            {
                yield return null;
            }
            else if (endLogic == EndLogic.Timed)
            {
                yield return new WaitForSeconds(duration);
            }

            Complete();
        }
    }
}