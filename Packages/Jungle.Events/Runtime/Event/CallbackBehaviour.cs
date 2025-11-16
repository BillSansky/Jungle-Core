using System;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Events
{
    public enum RegistrationTiming
    {
        Awake,
        Start,
        OnEnable
    }

    /// <summary>
    /// Base component capable of executing a configurable <see cref="IEventMonitor"/> and relaying the
    /// notification to serialized <see cref="IImmediateAction"/> instances and a <see cref="UnityEvent"/>.
    /// </summary>
    [AddComponentMenu("Jungle/Events/Callback Behaviour")]
    public class CallbackBehaviour : MonoBehaviour
    {
        [SerializeField]
        private RegistrationTiming registrationTiming = RegistrationTiming.OnEnable;

        [SerializeReference]
        [JungleClassSelection(typeof(IEventMonitor))]
        private IEventMonitor eventMonitor;

      

        [SerializeReference]
        [JungleClassSelection(typeof(IImmediateAction))]
        private List<IImmediateAction> callbackActions = new();

        [SerializeField]
        private UnityEvent onCallback = new();

        private bool isSubscribed;
        private Action callbackRelayAction;

        private void Awake()
        {
            if (registrationTiming == RegistrationTiming.Awake)
            {
                Subscribe();
            }
        }

        private void Start()
        {
            if (registrationTiming == RegistrationTiming.Start)
            {
                Subscribe();
            }
        }

        private void OnEnable()
        {
            if (registrationTiming == RegistrationTiming.OnEnable)
            {
                Subscribe();
            }
        }

        private void OnDisable()
        {
            if (!isSubscribed)
            {
                return;
            }

            eventMonitor.EndMonitoring();
            isSubscribed = false;
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

            EventMonitorUtilities.StartMonitoring(eventMonitor, callbackRelayAction);
            isSubscribed = true;
        }

        private void OnCallbackTriggered()
        {
            ExecuteCallbackActions();
            onCallback.Invoke();
        }

        private void ExecuteCallbackActions()
        {

            for (var index = 0; index < callbackActions.Count; index++)
            {
                var action = callbackActions[index];

                action.StartProcess();
            }
        }
        

    }
}
