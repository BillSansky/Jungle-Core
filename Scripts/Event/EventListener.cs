using System;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Events
{
    /// <summary>
    /// MonoBehaviour that listens to a <see cref="EventAsset"/> and relays the event to a UnityEvent response.
    /// </summary>
    [AddComponentMenu("Jungle/Events/Event Listener")]
    public class EventListener : MonoBehaviour
    {
        [SerializeField] private EventAsset eventAsset;
        [SerializeField] private UnityEvent response = new();

        private bool isRegistered;
        /// <summary>
        /// Handles the OnEnable event.
        /// </summary>
        private void OnEnable()
        {
            EnsureEventAssetAssigned();
            eventAsset.Register(OnEventRaised);
            isRegistered = true;
        }
        /// <summary>
        /// Handles the OnDisable event.
        /// </summary>
        private void OnDisable()
        {
            if (!isRegistered)
            {
                return;
            }

            eventAsset.Unregister(OnEventRaised);
            isRegistered = false;
        }
        /// <summary>
        /// Handles the OnEventRaised event.
        /// </summary>
        private void OnEventRaised()
        {
            response.Invoke();
        }
        /// <summary>
        /// Throws a descriptive error if the listener has not been wired to an event asset.
        /// </summary>
        private void EnsureEventAssetAssigned()
        {
            if (eventAsset == null)
            {
                throw new InvalidOperationException($"Event asset reference is not set on {nameof(EventListener)} attached to {name}.");
            }
        }
    }
}
