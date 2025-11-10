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

        private bool hasStarted;

        public void Start(Action callback = null)
        {
            if (hasStarted)
            {
                return;
            }

            RaiseEvent(enterEvent);

            hasStarted = true;
            callback?.Invoke();
        }
        /// <summary>
        /// Invoked when the state finishes running.
        /// </summary>

        public void Stop()
        {
            if (!hasStarted)
            {
                return;
            }

            RaiseEvent(exitEvent);

            hasStarted = false;
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
