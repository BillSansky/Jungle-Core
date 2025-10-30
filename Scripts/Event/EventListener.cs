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

        private void OnEnable()
        {
            EnsureEventAssetAssigned();
            eventAsset.Register(OnEventRaised);
            isRegistered = true;
        }

        private void OnDisable()
        {
            if (!isRegistered)
            {
                return;
            }

            eventAsset.Unregister(OnEventRaised);
            isRegistered = false;
        }

        private void OnEventRaised()
        {
            response.Invoke();
        }

        private void EnsureEventAssetAssigned()
        {
            if (eventAsset == null)
            {
                throw new InvalidOperationException($"Event asset reference is not set on {nameof(EventListener)} attached to {name}.");
            }
        }
    }
}
