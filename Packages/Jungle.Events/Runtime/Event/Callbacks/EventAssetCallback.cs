using System;
using System.Collections.Generic;
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

        private readonly List<Action> callbackActions = new();
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
        public void Attach(Action callbackAction)
        {
            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

            callbackActions.Add(callbackAction);
        }

        /// <inheritdoc />
        public void Detach(Action callbackAction)
        {
            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

            callbackActions.Remove(callbackAction);

            if (callbackActions.Count == 0)
            {
                EndMonitoring();
            }
        }

        /// <inheritdoc />
        public void StartMonitoring()
        {
            EnsureEventAssetAssigned();

            if (isListening || callbackActions.Count == 0)
            {
                return;
            }

            eventAsset.Register(OnEventRaised);
            isListening = true;
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            StopListening();
        }

        private void NotifyCallbackActions()
        {
            for (var index = 0; index < callbackActions.Count; index++)
            {
                callbackActions[index].Invoke();
            }
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
            NotifyCallbackActions();
            StopListening();
        }
    }
}
