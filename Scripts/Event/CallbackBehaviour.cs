using System;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Events
{
    /// <summary>
    /// Base component capable of executing a configurable <see cref="ICallback"/> and relaying the
    /// notification to serialized <see cref="IImmediateAction"/> instances and a <see cref="UnityEvent"/>.
    /// </summary>
    [AddComponentMenu("Jungle/Events/Callback Behaviour")]
    public class CallbackBehaviour : MonoBehaviour
    {
        [SerializeReference]
        [JungleClassSelection(typeof(ICallback))]
        private ICallback callback;

        [SerializeField]
        private bool invokeOnEnable;

        [SerializeReference]
        [JungleClassSelection(typeof(IImmediateAction))]
        private List<IImmediateAction> callbackActions = new();

        [SerializeField]
        private UnityEvent onCallback = new();

        private bool isSubscribed;
        private Action callbackRelayAction;

        private void Awake()
        {
            EnsureCallbackAssigned();
        }

        private void OnEnable()
        {
            EnsureCallbackAssigned();
            Subscribe();

            if (invokeOnEnable)
            {
                InvokeCallback();
            }
        }

        private void OnDisable()
        {
            if (!isSubscribed)
            {
                return;
            }

            callback.Detach(callbackRelayAction);
            isSubscribed = false;
        }

        /// <summary>
        /// Manually invokes the configured callback.
        /// This can be triggered from other scripts or via UnityEvents.
        /// </summary>
        public void InvokeCallback()
        {
            EnsureCallbackAssigned();
            callback.Invoke();
        }

        private void EnsureCallbackAssigned()
        {
            if (callback == null)
            {
                throw new InvalidOperationException($"Callback reference is not configured on {name}.");
            }
        }

        private void Subscribe()
        {
            if (isSubscribed)
            {
                return;
            }

            if (callbackRelayAction == null)
            {
                callbackRelayAction = OnCallbackTriggered;
            }

            callback.Attach(callbackRelayAction);
            isSubscribed = true;
        }

        private void OnCallbackTriggered()
        {
            ExecuteCallbackActions();
            onCallback.Invoke();
        }

        private void ExecuteCallbackActions()
        {
            if (callbackActions == null)
            {
                Debug.LogError(
                    $"Callback actions list on {name} resolved to null. Reassign the field on {nameof(CallbackBehaviour)}.",
                    this);
                return;
            }

            for (var index = 0; index < callbackActions.Count; index++)
            {
                var action = callbackActions[index];
                if (action == null)
                {
                    Debug.LogError(
                        $"CallbackBehaviour on {name} contains a null callback action at index {index}.",
                        this);
                    continue;
                }

                action.Execute();
            }
        }

#if UNITY_EDITOR
        private void Reset()
        {
            invokeOnEnable = false;
        }
#endif

    }
}
