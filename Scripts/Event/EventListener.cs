using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Attributes;
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

        [SerializeReference]
        [JungleClassSelection(typeof(IImmediateAction))]
        private List<IImmediateAction> eventActions = new();

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
            ExecuteEventActions();
            response.Invoke();
        }

        private void ExecuteEventActions()
        {
            if (eventActions == null)
            {
                Debug.LogError(
                    $"Event actions list on {name} resolved to null. Reassign the field on {nameof(EventListener)}.",
                    this);
                return;
            }

            for (var index = 0; index < eventActions.Count; index++)
            {
                var action = eventActions[index];
                if (action == null)
                {
                    Debug.LogError(
                        $"EventListener on {name} contains a null event action at index {index}.",
                        this);
                    continue;
                }

                action.Execute();
            }
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
