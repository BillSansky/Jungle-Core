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
        [SerializeField] private EventAsset enterEvent;
        [SerializeField] private EventAsset exitEvent;

        /// <summary>
        /// Raises the configured enter event when the state begins.
        /// </summary>
        public void OnStateEnter()
        {
            RaiseEvent(enterEvent);
        }
        /// <summary>
        /// Raises the configured exit event when the state finishes.
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
