using System;
using Jungle.Attributes;
using Jungle.Events;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Raises a Jungle Event asset when executed.
    /// </summary>
    [JungleClassInfo("Event Invoke Action", "Raises a Jungle Event asset when executed.", "d_UnityEvent Icon", "Actions/Events")]
    [Serializable]
    public class EventInvokeAction : IImmediateAction
    {
        /// <summary>
        /// Event asset that will be raised when the action runs.
        /// </summary>
        [SerializeField] private EventAsset eventAsset;

        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => true;

        public float Duration => 0f;

        public bool IsInProgress => false;

        public bool HasCompleted => hasCompleted;

        public void StartProcess(Action callback = null)
        {
            hasCompleted = false;
            Execute();
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            callback?.Invoke();
        }

        public void Stop()
        {
            hasCompleted = false;
        }

        public void Execute()
        {
            EnsureEventAssetAssigned();
            eventAsset.Raise();
        }

        private void EnsureEventAssetAssigned()
        {
            if (eventAsset == null)
            {
                throw new InvalidOperationException($"Event asset reference is not set on {nameof(EventInvokeAction)}.");
            }
        }
    }
}
