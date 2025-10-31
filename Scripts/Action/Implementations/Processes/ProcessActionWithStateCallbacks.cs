using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Base class that surfaces state entry and exit callbacks to derived process actions.
    /// </summary>
    [Serializable]
    public class ProcessActionWithStateCallbacks : IProcessAction
    {
        [SerializeReference][JungleClassSelection]
        public IProcessAction ProcessAction;

        [SerializeReference][JungleClassSelection]
        public List<IStateAction> StateActions = new List<IStateAction>();

        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => ProcessAction?.HasDefinedDuration ?? false;

        public float Duration => ProcessAction?.Duration ?? 0.0f;

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;
        /// <summary>
        /// Begins the wrapped process action, matching the <see cref="IProcessAction.Execute"/> contract.
        /// </summary>
        public void Execute()
        {
            Start();
        }
        /// <summary>
        /// Starts the nested process and fires OnStateEnter on every linked state action.
        /// </summary>
        public void Start()
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
                ProcessAction.OnProcessCompleted += OnNestedProcessCompleted;
                ProcessAction.Start();
            }
            else
            {
                // If no process action, complete immediately
                Complete();
            }
        }
        /// <summary>
        /// Stops the running process and calls OnStateExit on each state action.
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
                ProcessAction.OnProcessCompleted -= OnNestedProcessCompleted;
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
        /// <summary>
        /// Handles the OnNestedProcessCompleted event.
        /// </summary>
        private void OnNestedProcessCompleted()
        {
            Complete();
        }
        /// <summary>
        /// Finalizes the process run, unsubscribes from callbacks, and notifies listeners of completion.
        /// </summary>
        private void Complete()
        {
            if (!isInProgress)
            {
                return;
            }

            // Unsubscribe from nested process
            if (ProcessAction != null)
            {
                ProcessAction.OnProcessCompleted -= OnNestedProcessCompleted;
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
