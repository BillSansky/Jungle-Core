using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Waits for a mouse button interaction before notifying callback actions.
    /// </summary>
    [Serializable]
    public sealed class MouseButtonCallback : IEventMonitor
    {
        [SerializeField]
        [Tooltip("Index of the mouse button to monitor. 0 = Left, 1 = Right, 2 = Middle.")]
        private int mouseButtonIndex;

        [SerializeField]
        private InputInteraction interaction = InputInteraction.ButtonDown;

        private readonly List<Action> callbackActions = new();
        private Coroutine routine;

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
            if (callbackActions.Count == 0)
            {
                return;
            }

            EnsureValidButtonIndex();

            EndMonitoring();
            routine = CoroutineRunner.StartManagedCoroutine(WaitForMouse());
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            if (routine == null)
            {
                return;
            }

            CoroutineRunner.StopManagedCoroutine(routine);
            routine = null;
        }

        private IEnumerator WaitForMouse()
        {
            while (true)
            {
                yield return null;

                if (IsTriggered())
                {
                    NotifyCallbackActions();
                    routine = null;
                    yield break;
                }
            }
        }

        private bool IsTriggered()
        {
            return interaction switch
            {
                InputInteraction.ButtonDown => Input.GetMouseButtonDown(mouseButtonIndex),
                InputInteraction.ButtonHeld => Input.GetMouseButton(mouseButtonIndex),
                InputInteraction.ButtonUp => Input.GetMouseButtonUp(mouseButtonIndex),
                _ => false
            };
        }

        private void EnsureValidButtonIndex()
        {
            if (mouseButtonIndex < 0 || mouseButtonIndex > 6)
            {
                throw new InvalidOperationException(
                    $"{nameof(MouseButtonCallback)} expects a button index between 0 and 6. Value {mouseButtonIndex} is outside the supported range.");
            }
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
