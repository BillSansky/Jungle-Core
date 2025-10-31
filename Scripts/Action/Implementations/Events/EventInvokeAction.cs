using System;
using Jungle.Attributes;
using Jungle.Events;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Invokes a UnityEvent when the state machine reaches the step.
    /// </summary>
    [JungleClassInfo("Raises a Jungle Event asset when executed.", "d_UnityEvent Icon")]
    [Serializable]
    public class EventInvokeAction : IImmediateAction
    {
        /// <summary>
        /// Raises the configured event asset.
        /// </summary>
        [SerializeField] private EventAsset eventAsset;

        public void Execute()
        {
            EnsureEventAssetAssigned();
            eventAsset.Raise();
        }
        /// <summary>
        /// Verifies that an event asset has been assigned before execution.
        /// </summary>
        private void EnsureEventAssetAssigned()
        {
            if (eventAsset == null)
            {
                throw new InvalidOperationException($"Event asset reference is not set on {nameof(EventInvokeAction)}.");
            }
        }
    }
}
