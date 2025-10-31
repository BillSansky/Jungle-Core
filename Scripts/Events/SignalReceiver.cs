using System;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Events
{
    /// <summary>
    /// Component capable of reacting to <see cref="SignalType"/> notifications.
    /// It maps each signal to a list of <see cref="IImmediateAction"/> instances and
    /// a <see cref="UnityEvent"/> that are executed when the signal is received.
    /// </summary>
    [DisallowMultipleComponent]
    public class SignalReceiver : MonoBehaviour
    {
        /// <summary>
        /// Serializes the signal configuration and responses for a specific SignalType.
        /// </summary>
        [Serializable]
        private class SignalHandler
        {
            [SerializeField]
            private SignalType signalType;

            [SerializeReference]
            [JungleClassSelection(typeof(IImmediateAction))]
            private List<IImmediateAction> immediateActions = new List<IImmediateAction>();

            [SerializeField]
            private UnityEvent onSignal = new UnityEvent();
            /// <summary>
            /// Determines whether this handler should respond to the incoming signal.
            /// </summary>
            public bool Handles(SignalType incomingSignal)
            {
                return signalType == incomingSignal;
            }
            /// <summary>
            /// Runs each configured action and UnityEvent in response to the signal.
            /// </summary>
            public void InvokeActions(SignalReceiver receiver, SignalType incomingSignal)
            {
                for (var index = 0; index < immediateActions.Count; index++)
                {
                    var action = immediateActions[index];
                    if (action == null)
                    {
                        Debug.LogError(
                            $"Signal handler for '{incomingSignal.name}' on {receiver.name} contains a null action " +
                            $"at index {index}. Review the serialized actions on {receiver.name}.",
                            receiver);
                        continue;
                    }

                    action.Execute();
                }

                onSignal?.Invoke();
            }
        }

        [SerializeField]
        private List<SignalHandler> handlers = new List<SignalHandler>();

        /// <summary>
        /// Receives the provided <see cref="SignalType"/> and triggers any configured actions/events.
        /// </summary>
        /// <param name="signalType">The signal to process.</param>
        public void Receive(SignalType signalType)
        {
            if (signalType == null)
            {
                Debug.LogError(
                    $"A null {nameof(SignalType)} was provided to {nameof(SignalReceiver)} on {name}. " +
                    "Ensure a valid asset is passed when invoking Receive().",
                    this);
                return;
            }

            var handled = false;
            foreach (var handler in handlers)
            {
                if (handler == null)
                {
                    Debug.LogError(
                        $"SignalReceiver on {name} has an unassigned handler entry in its configuration.",
                        this);
                    continue;
                }

                if (!handler.Handles(signalType))
                {
                    continue;
                }

                handled = true;
                handler.InvokeActions(this, signalType);
            }

            if (!handled)
            {
                Debug.LogWarning(
                    $"SignalReceiver on {name} does not have any handler configured for signal '{signalType.name}'.",
                    this);
            }
        }

        /// <summary>
        /// Attempts to locate a <see cref="SignalReceiver"/> component starting from the provided GameObject.
        /// </summary>
        /// <param name="source">GameObject that should contain the receiver.</param>
        /// <param name="receiver">The located receiver, if any.</param>
        /// <param name="searchParents">When true, also searches the parent hierarchy.</param>
        /// <param name="searchChildren">When true, also searches the children hierarchy.</param>
        /// <param name="includeInactiveChildren">When searching children, determines if inactive children are considered.</param>
        /// <returns>True when a receiver is found, otherwise false.</returns>
        public static bool TryGetReceiver(
            GameObject source,
            out SignalReceiver receiver,
            bool searchParents = false,
            bool searchChildren = false,
            bool includeInactiveChildren = false)
        {
            if (source == null)
            {
                Debug.LogError(
                    "Cannot look for a SignalReceiver because the provided GameObject reference resolved to null.");
                receiver = null;
                return false;
            }

            receiver = source.GetComponent<SignalReceiver>();
            if (receiver != null)
            {
                return true;
            }

            if (searchParents)
            {
                receiver = source.GetComponentInParent<SignalReceiver>(true);
                if (receiver != null)
                {
                    return true;
                }
            }

            if (searchChildren)
            {
                receiver = source.GetComponentInChildren<SignalReceiver>(includeInactiveChildren);
                if (receiver != null)
                {
                    return true;
                }
            }

            receiver = null;
            return false;
        }
    }
}

