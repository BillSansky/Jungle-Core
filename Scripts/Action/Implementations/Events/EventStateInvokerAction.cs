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
    public class EventStateInvokerAction : IStateAction
    {
        /// <summary>
        /// Invoked when the state becomes active.
        /// </summary>
        [SerializeField] private EventAsset enterEvent;
        [SerializeField] private EventAsset exitEvent;

        public void OnStateEnter()
        {
            RaiseEvent(enterEvent);
        }
        /// <summary>
        /// Invoked when the state finishes running.
        /// </summary>

        public void OnStateExit()
        {
            RaiseEvent(exitEvent);
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
