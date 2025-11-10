using System;
using Jungle.Attributes;
using Jungle.Events;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Raises Jungle Event assets when the state enters or exits.
    /// </summary>
    [JungleClassInfo("Event State Invoker", "Raises Jungle Event assets when the state enters or exits.", "d_UnityEvent Icon", "Actions/Events")]
    [Serializable]
    public class EventStateInvokerAction : IImmediateAction
    {
        /// <summary>
        /// Invoked when the state becomes active.
        /// </summary>
        [SerializeField] private EventAsset enterEvent;
        [SerializeField] private EventAsset exitEvent;

        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => true;

        public float Duration => 0f;

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;

        public void Start(Action callback = null)
        {
            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;

            RaiseEvent(enterEvent);

            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            callback?.Invoke();
        }
        /// <summary>
        /// Invoked when the state finishes running.
        /// </summary>

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            RaiseEvent(exitEvent);

            isInProgress = false;
            hasCompleted = false;
        }

        private static void RaiseEvent(EventAsset eventAsset)
        {
            if (eventAsset == null)
            {
                return;
            }

            eventAsset.Raise();
        }
    }
}
