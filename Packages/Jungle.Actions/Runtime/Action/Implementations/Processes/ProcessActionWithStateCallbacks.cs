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
        /// Defines the immediate actions invoked on start and completion.
        /// </summary>

        [SerializeReference][JungleClassSelection]
        public List<IImmediateAction> StateActions = new List<IImmediateAction>();

        private bool isInProgress;
        private bool hasCompleted;
        private Action completionCallback;

        public event Action OnProcessCompleted;

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
        /// Executes the process immediately by delegating to <see cref="StartProcess"/>.
        /// </summary>

        public void Execute()
        {
            StartProcess(null);
        }

        /// <summary>
        /// Starts the process action with state callbacks.
        /// </summary>
        /// <param name="callback"></param>
        public void StartProcess(Action callback = null)
        {
            Debug.Assert(ProcessAction != null, "ProcessAction must be set before starting");

            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;
            completionCallback = callback;

            // Execute immediate actions enter
            foreach (var action in StateActions)
            {
                action?.StartProcess();
            }

            // Subscribe to nested process completion
            if (ProcessAction != null)
            {
                ProcessAction.StartProcess(OnNestedProcessCompleted);
            }
            else
            {
                // If no process action, complete immediately
                Complete();
            }
        }
        /// <summary>
        /// Stops the process action with state callbacks before completion.
        /// </summary>

        public void Stop()
        {
            if (!isInProgress)
            {
                return;
            }

            // Unsubscribe from nested process
            if (ProcessAction != null)
            {
                ProcessAction.Stop();
            }

            // Exit immediate actions
            foreach (var action in StateActions)
            {
                action?.Stop();
            }

            isInProgress = false;
            hasCompleted = false;
            completionCallback = null;
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

            // Exit immediate actions
            foreach (var action in StateActions)
            {
                action?.Stop();
            }

            isInProgress = false;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            completionCallback?.Invoke();
            completionCallback = null;
        }
    }
}
