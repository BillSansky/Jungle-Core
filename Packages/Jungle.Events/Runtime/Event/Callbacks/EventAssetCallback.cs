using System;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Waits until a <see cref="EventAsset"/> raises before notifying callback actions.
    /// </summary>
    [Serializable]
    public sealed class EventAssetCallback : IEventMonitor
    {
        [SerializeField]
        private EventAsset eventAsset;

        private Action callbackAction;
        private bool isListening;

        /// <summary>
        /// Creates an empty callback. Use <see cref="AssignEventAsset"/> before invoking.
        /// </summary>
        public EventAssetCallback()
        {
        }

        /// <summary>
        /// Creates a callback that listens to the provided <paramref name="asset"/>.
        /// </summary>
        public EventAssetCallback(EventAsset asset)
        {
            AssignEventAsset(asset);
        }

        /// <summary>
        /// Assigns the event asset that should be monitored.
        /// </summary>
        /// <param name="asset">Event asset to observe.</param>
        public void AssignEventAsset(EventAsset asset)
        {
            eventAsset = asset ?? throw new ArgumentNullException(nameof(asset));
        }

        /// <inheritdoc />
        public void StartMonitoring(Action callbackAction)
        {
            EnsureEventAssetAssigned();

            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

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

        private void EnsureEventAssetAssigned()
        {
            if (eventAsset == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(EventAssetCallback)} requires an {nameof(EventAsset)} reference before invocation.");
            }
        }

        private void StopListening()
        {
            if (!isListening)
            {
                return;
            }

            EnsureEventAssetAssigned();
            eventAsset.Unregister(OnEventRaised);

            isListening = false;
        }

        private void OnEventRaised()
        {
            var action = callbackAction;
            callbackAction = null;
            StopListening();
            action?.Invoke();
        }
    }
}
