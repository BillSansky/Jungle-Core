using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Events
{
    /// <summary>
    /// Waits for a parameterless <see cref="UnityEvent"/> defined on a <see cref="MonoBehaviour"/> to raise
    /// before notifying callback actions.
    /// </summary>
    [Serializable]
    public sealed class MonoBehaviourEventCallback : ICallback
    {
        [SerializeField]
        private MonoBehaviour eventSource;

        [SerializeField]
        private string unityEventName;

        private readonly List<Action> callbackActions = new();
        private UnityEvent activeUnityEvent;
        private UnityAction relayAction;
        private bool isListening;

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
                StopListening();
            }
        }

        /// <inheritdoc />
        public void Invoke()
        {
            if (callbackActions.Count == 0)
            {
                return;
            }

            var unityEvent = ResolveUnityEvent();

            if (isListening && activeUnityEvent == unityEvent)
            {
                return;
            }

            if (relayAction == null)
            {
                relayAction = OnUnityEventRaised;
            }

            StopListening();

            unityEvent.AddListener(relayAction);
            activeUnityEvent = unityEvent;
            isListening = true;
        }

        private UnityEvent ResolveUnityEvent()
        {
            if (eventSource == null)
            {
                throw new InvalidOperationException(
                    $"{nameof(MonoBehaviourEventCallback)} requires an event source before invocation.");
            }

            if (string.IsNullOrWhiteSpace(unityEventName))
            {
                throw new InvalidOperationException(
                    $"{nameof(MonoBehaviourEventCallback)} requires the UnityEvent name to be specified before invocation.");
            }

            var memberFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var sourceType = eventSource.GetType();

            var field = sourceType.GetField(unityEventName, memberFlags);
            if (field != null)
            {
                return ExtractUnityEvent(field.GetValue(eventSource), field.FieldType, unityEventName);
            }

            var property = sourceType.GetProperty(unityEventName, memberFlags);
            if (property != null)
            {
                if (!property.CanRead)
                {
                    throw new InvalidOperationException(
                        $"UnityEvent property '{unityEventName}' on {sourceType.Name} does not expose a getter.");
                }

                if (property.GetIndexParameters().Length > 0)
                {
                    throw new InvalidOperationException(
                        $"UnityEvent property '{unityEventName}' on {sourceType.Name} cannot be an indexer.");
                }

                return ExtractUnityEvent(property.GetValue(eventSource), property.PropertyType, unityEventName);
            }

            throw new InvalidOperationException(
                $"UnityEvent '{unityEventName}' was not found on {sourceType.Name}. Ensure the field or property exists and is of type {nameof(UnityEvent)}.");
        }

        private UnityEvent ExtractUnityEvent(object memberValue, Type memberType, string memberName)
        {
            if (!typeof(UnityEvent).IsAssignableFrom(memberType))
            {
                throw new InvalidOperationException(
                    $"Member '{memberName}' is of type {memberType.Name} and cannot be used with {nameof(MonoBehaviourEventCallback)}. Provide a parameterless {nameof(UnityEvent)} instead.");
            }

            if (memberValue is not UnityEvent unityEvent)
            {
                throw new InvalidOperationException(
                    $"Member '{memberName}' on {eventSource.GetType().Name} resolved to null. Assign a {nameof(UnityEvent)} instance before invoking.");
            }

            return unityEvent;
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
            NotifyCallbackActions();
            StopListening();
        }

        private void NotifyCallbackActions()
        {
            for (var index = 0; index < callbackActions.Count; index++)
            {
                callbackActions[index].Invoke();
            }
        }
    }
}
