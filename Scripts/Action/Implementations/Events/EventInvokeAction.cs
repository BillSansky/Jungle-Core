using System;
using Jungle.Attributes;
using Jungle.Events;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Raises a Jungle Event asset when executed.", "d_UnityEvent Icon")]
    [Serializable]
    public class EventInvokeAction : IImmediateAction
    {
        [SerializeField] private EventAsset eventAsset;

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
