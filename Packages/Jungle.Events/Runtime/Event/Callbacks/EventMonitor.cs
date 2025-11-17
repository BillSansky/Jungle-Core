using System;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Waits until a <see cref="EventAsset"/> raises before notifying callback actions.
    /// </summary>
    [Serializable]
    public sealed class EventMonitor : IEventMonitor
    {
        [SerializeField]
        private EventAsset eventAsset;

        private Action callbackAction;
        private bool isListening;
        

        /// <inheritdoc />
        public void StartMonitoring(Action callbackAction)
        {
            
            StopListening();
            this.callbackAction = callbackAction;
            eventAsset.Register(OnEventRaised);
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
            
            eventAsset.Unregister(OnEventRaised);

            isListening = false;
        }

        private void OnEventRaised()
        {
            callbackAction.Invoke();
        }
    }
}
