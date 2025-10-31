using System;
using Jungle.Attributes;
using Jungle.Events;
using Jungle.Values;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Broadcasts the configured signal through the central dispatcher when executed.
    /// </summary>
    [JungleClassInfo("Sends a signal to the target receiver when executed.", "d_EventSystem Icon")]
    [Serializable]
    public class SignalInvokeAction : IImmediateAction
    {
        [SerializeField]
        private SignalType signalType;

        [SerializeReference]
        [JungleClassSelection(typeof(IGameObjectReference))]
        private IGameObjectReference receiverReference = new GameObjectValue();

        [SerializeField]
        private bool searchParents;

        [SerializeField]
        private bool searchChildren;

        [SerializeField]
        private bool includeInactiveChildren;
        /// <summary>
        /// Resolves the target receiver and forwards the configured signal.
        /// </summary>
        public void Execute()
        {
            if (signalType == null)
            {
                Debug.LogError(
                    $"SignalInvokeAction requires a {nameof(SignalType)} reference before it can execute.");
                return;
            }

            if (receiverReference == null)
            {
                Debug.LogError(
                    "SignalInvokeAction requires a receiver reference to resolve the target GameObject.");
                return;
            }

            var target = receiverReference.GameObject;
            if (target == null)
            {
                Debug.LogError(
                    "The configured receiver reference resolved to a null GameObject. " +
                    "Ensure the reference points to a valid object containing a SignalReceiver component.");
                return;
            }

            if (!SignalReceiver.TryGetReceiver(target, out var receiver, searchParents, searchChildren, includeInactiveChildren))
            {
                Debug.LogError(
                    $"No SignalReceiver component could be found for signal '{signalType.name}' on target '{target.name}'.");
                return;
            }

            receiver.Receive(signalType);
        }
    }
}

