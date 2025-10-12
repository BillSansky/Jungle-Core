using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
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

        public void Execute()
        {
            Start();
        }

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
