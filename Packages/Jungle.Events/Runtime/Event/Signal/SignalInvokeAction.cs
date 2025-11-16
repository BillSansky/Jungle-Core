using System;
using Jungle.Attributes;
using Jungle.Events;
using Jungle.Values;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Sends a signal to the target receiver when executed.
    /// </summary>
    [JungleClassInfo("Signal Invoke Action", "Sends a signal to the target receiver when executed.", "d_EventSystem Icon", "Actions/Events")]
    [Serializable]
    public class SignalInvokeAction : IImmediateAction
    {
        [SerializeField]
        private SignalType signalType;

        [SerializeReference]
        [JungleClassSelection(typeof(IGameObjectValue))]
        private IGameObjectValue receiverObject = new GameObjectValue();

        [SerializeField]
        private bool searchParents;

        [SerializeField]
        private bool searchChildren;

        [SerializeField]
        private bool includeInactiveChildren;

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
        /// <summary>
        /// Sends the configured signal to the resolved receiver immediately.
        /// </summary>

        public void Execute()
        {
            if (signalType == null)
            {
                Debug.LogError(
                    $"SignalInvokeAction requires a {nameof(SignalType)} reference before it can execute.");
                return;
            }

            if (receiverObject == null)
            {
                Debug.LogError(
                    "SignalInvokeAction requires a receiver reference to resolve the target GameObject.");
                return;
            }

            var target = receiverObject.V;
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

