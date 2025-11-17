using System;
using System.Reflection;
using Jungle.Values.GameDev;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Events
{
    /// <summary>
    /// Waits for a parameterless <see cref="UnityEvent"/> defined on a <see cref="MonoBehaviour"/> to raise
    /// before notifying callback actions.
    /// </summary>
    [Serializable]
    public sealed class ComponentEventCallback : IEventMonitor
    {
        [SerializeField]
        private IComponentValue eventSource;

        
        
        [SerializeField]
        private string unityEventName;

        private Action callbackAction;
        private UnityEvent activeUnityEvent;
        private UnityAction relayAction;
        private bool isListening;

        /// <inheritdoc />
        public void StartMonitoring(Action callbackAction)
        {

            var unityEvent = ResolveUnityEvent();

            if (isListening && activeUnityEvent == unityEvent)
            {
                this.callbackAction = callbackAction;
                return;
            }

            if (relayAction == null)
            {
                relayAction = OnUnityEventRaised;
            }

            StopListening();

            this.callbackAction = callbackAction;
            unityEvent.AddListener(relayAction);
            activeUnityEvent = unityEvent;
            isListening = true;
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            StopListening();
            callbackAction = null;
        }

       
        private void StopListening()
        {
            if (!isListening)
            {
                return;
            }

            if (relayAction != null && activeUnityEvent != null)
            {
                activeUnityEvent.RemoveListener(relayAction);
            }

            activeUnityEvent = null;
            isListening = false;
        }

        private void OnUnityEventRaised()
        {
            var action = callbackAction;
            callbackAction = null;
            StopListening();
            action?.Invoke();
        }
    }
}
