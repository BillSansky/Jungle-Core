using System;
using Jungle.Attributes;
using Jungle.Events;
using Jungle.Values;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Sends signals when the state enters and exits.
    /// </summary>
    [JungleClassInfo("Signal State Invoker", "Sends signals when the state enters and exits.", "d_EventSystem Icon", "Actions/Events")]
    [Serializable]
    public class SignalStateInvokerAction : IImmediateAction
    {
        [SerializeField]
        private SignalType enterSignal;

        [SerializeField]
        private SignalType exitSignal;

        [SerializeReference]
        [JungleClassSelection(typeof(IGameObjectReference))]
        private IGameObjectReference receiverReference = new GameObjectValue();

        [SerializeField]
        private bool searchParents;

        [SerializeField]
        private bool searchChildren;

        [SerializeField]
        private bool includeInactiveChildren;

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

            InvokeSignal(enterSignal, "enter");

            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            callback?.Invoke();
        }

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            InvokeSignal(exitSignal, "exit");

            isInProgress = false;
            hasCompleted = false;
        }

        private void InvokeSignal(SignalType signal, string phase)
        {
            if (signal == null)
            {
                return;
            }

            if (receiverReference == null)
            {
                Debug.LogError(
                    $"SignalStateInvokerAction requires a receiver reference to resolve the target GameObject for the {phase} phase.");
                return;
            }

            var target = receiverReference.GameObject;
            if (target == null)
            {
                Debug.LogError(
                    $"The configured receiver reference resolved to a null GameObject during the {phase} phase. " +
                    "Ensure the reference points to a valid object containing a SignalReceiver component.");
                return;
            }

            if (!SignalReceiver.TryGetReceiver(target, out var receiver, searchParents, searchChildren, includeInactiveChildren))
            {
                Debug.LogError(
                    $"No SignalReceiver component could be found for signal '{signal.name}' on target '{target.name}' during the {phase} phase.");
                return;
            }

            receiver.Receive(signal);
        }
    }
}

