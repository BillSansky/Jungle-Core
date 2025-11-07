using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Runs another process while invoking state actions on start and stop.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Process With State Callbacks", "Runs another process while invoking state actions on start and stop.", null, "Actions/Process")]
    public class ProcessActionWithStateCallbacks : IProcessAction
    {
        /// <summary>
        /// <summary>
        /// Gets or sets the wrapped process action.
        /// </summary>
        [SerializeReference][JungleClassSelection]
        public IProcessAction ProcessAction;
        /// <summary>
        /// Defines the state actions invoked on start and completion.
        /// </summary>

        [SerializeReference][JungleClassSelection]
        public List<IStateAction> StateActions = new List<IStateAction>();

        private bool isInProgress;
        private bool hasCompleted;

        /// <summary>
        /// Indicates whether the action can report a finite duration.
        /// </summary>

        public bool HasDefinedDuration => ProcessAction?.HasDefinedDuration ?? false;
        /// <summary>
        /// Gets the total duration of the action in seconds.
        /// </summary>

        public float Duration => ProcessAction?.Duration ?? 0.0f;
        /// <summary>
        /// Gets whether the action is currently running.
        /// </summary>

        public bool IsInProgress => isInProgress;
        /// <summary>
        /// Gets whether the action has finished executing.
        /// </summary>

        public bool HasCompleted => hasCompleted;
        /// <summary>
        /// Executes the process immediately by delegating to <see cref="Start"/>.
        /// </summary>

        public void Execute()
        {
            Start(null);
        }

        /// <summary>
        /// Starts the process action with state callbacks.
        /// </summary>
        /// <param name="callback"></param>
        public void Start(Action callback)
        {
            Debug.Assert(ProcessAction != null, "ProcessAction must be set before starting");

            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;

            // Execute state actions enter
            foreach (var action in StateActions)
            {
                action?.OnStateEnter();
            }

            // Subscribe to nested process completion
            if (ProcessAction != null)
            {
                ProcessAction.Start(null);
            }
            else
            {
                // If no process action, complete immediately
                Complete();
            }
        }
        /// <summary>
        /// Interrupts the process action with state callbacks before completion.
        /// </summary>

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            // Unsubscribe from nested process
            if (ProcessAction != null)
            {
                ProcessAction.Interrupt();
            }

            // Exit state actions
            foreach (var action in StateActions)
            {
                action?.OnStateExit();
            }

            isInProgress = false;
            hasCompleted = false;
        }

        private void OnNestedProcessCompleted()
        {
            Complete();
        }

        private void Complete()
        {
            if (!isInProgress)
            {
                return;
            }

            // Unsubscribe from nested process
            if (ProcessAction != null)
            {
            }

            // Exit state actions
            foreach (var action in StateActions)
            {
                action?.OnStateExit();
            }

            isInProgress = false;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
        }
    }
}
