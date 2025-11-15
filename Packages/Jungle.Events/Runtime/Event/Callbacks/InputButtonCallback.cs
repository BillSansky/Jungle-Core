using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Indicates which Input system interaction should trigger the callback.
    /// </summary>
    public enum InputInteraction
    {
        ButtonDown,
        ButtonHeld,
        ButtonUp
    }

    /// <summary>
    /// Waits for a Unity Input Manager button interaction before notifying callback actions.
    /// </summary>
    [Serializable]
    public sealed class InputButtonCallback : IEventMonitor
    {
        [SerializeField]
        private string buttonName = "Jump";

        [SerializeField]
        private InputInteraction interaction = InputInteraction.ButtonDown;

        private readonly List<Action> callbackActions = new();
        private Coroutine routine;

        /// <summary>
        /// Name of the button configured in the Unity Input Manager.
        /// </summary>
        public string ButtonName
        {
            get => buttonName;
            set => buttonName = value;
        }

        /// <summary>
        /// Which interaction should complete the callback.
        /// </summary>
        public InputInteraction Interaction
        {
            get => interaction;
            set => interaction = value;
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
            if (callbackActions.Count == 0)
            {
                return;
            }

            EnsureButtonConfigured();

            EndMonitoring();
            routine = CoroutineRunner.StartManagedCoroutine(WaitForInput());
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

        private IEnumerator WaitForInput()
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
                InputInteraction.ButtonDown => Input.GetButtonDown(buttonName),
                InputInteraction.ButtonHeld => Input.GetButton(buttonName),
                InputInteraction.ButtonUp => Input.GetButtonUp(buttonName),
                _ => false
            };
        }

        private void EnsureButtonConfigured()
        {
            if (string.IsNullOrWhiteSpace(buttonName))
            {
                throw new InvalidOperationException(
                    $"{nameof(InputButtonCallback)} requires a valid Input Manager button name.");
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
