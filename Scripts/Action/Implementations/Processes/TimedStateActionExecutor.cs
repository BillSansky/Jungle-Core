using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Determines whether the executor completes after a duration or on the next frame.
    /// </summary>
    public enum EndLogic
    {
        Timed,
        OneFrame,
    }
    /// <summary>
    /// Runs an action for a fixed duration and reports progress back to the state runner.
    /// </summary>
    [Serializable]
    public class TimedStateActionExecutor : IProcessAction
    {
        
        [SerializeReference][JungleClassSelection]
        public List<IStateAction> Actions  = new List<IStateAction>();
        
        public EndLogic endLogic = EndLogic.Timed;
        
        public float duration = 1.0f;

        [NonSerialized]
        public MonoBehaviour coroutineRunner;

        private Coroutine autoEndCoroutine;
        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => true;

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

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;
        /// <summary>
        /// Invokes <see cref="Start"/> so the executor can be triggered through the process interface.
        /// </summary>
        public void Execute()
        {
            Start();
        }
        /// <summary>
        /// Enters each state action and optionally schedules an automatic completion.
        /// </summary>
        public void Start()
        {
            Debug.Assert(coroutineRunner != null, "coroutineRunner must be set before starting the action");

            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;

            foreach (var action in Actions)
            {
                action?.OnStateEnter();
            }

            // Start auto-end coroutine if needed
            if ((endLogic == EndLogic.Timed || endLogic == EndLogic.OneFrame) && coroutineRunner != null)
            {
                autoEndCoroutine = coroutineRunner.StartCoroutine(AutoEndCoroutine());
            }
        }
        /// <summary>
        /// Stops the executor early, reversing state entries and cancelling timers.
        /// </summary>
        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            Cleanup();
            isInProgress = false;
            hasCompleted = false;
        }
        /// <summary>
        /// Cancels the auto-end coroutine and calls OnStateExit on all actions.
        /// </summary>
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
                action?.OnStateExit();
            }
        }
        /// <summary>
        /// Finishes the run, performs cleanup, and fires the completion event.
        /// </summary>
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
        }
        /// <summary>
        /// Waits for the configured duration (or one frame) before automatically completing the executor.
        /// </summary>
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