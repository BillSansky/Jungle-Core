using System;
using Jungle.Attributes;
using Jungle.Events;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Invokes a UnityEvent and waits for it to complete before finishing the state transition.
    /// </summary>
    [JungleClassInfo("Raises Jungle Event assets when the state enters or exits.", "d_UnityEvent Icon")]
    [Serializable]
    public class EventStateInvokerAction : IStateAction
    {
        /// <summary>
        /// Handles the OnStateEnter event.
        /// </summary>
        [SerializeField] private EventAsset enterEvent;
        [SerializeField] private EventAsset exitEvent;

        public void OnStateEnter()
        {
            RaiseEvent(enterEvent);
        }
        /// <summary>
        /// Handles the OnStateExit event.
        /// </summary>
        public void OnStateExit()
        {
            RaiseEvent(exitEvent);
        }
        /// <summary>
        /// Safely raises the provided event asset if one is assigned.
        /// </summary>
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
