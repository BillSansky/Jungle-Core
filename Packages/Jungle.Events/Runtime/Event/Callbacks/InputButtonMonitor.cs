using System;
using System.Collections;
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
    public sealed class InputButtonMonitor : IEventMonitor
    {
        [SerializeField] private string buttonName = "Jump";

        [SerializeField] private InputInteraction interaction = InputInteraction.ButtonDown;

        private Action callbackAction;
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
        public void StartMonitoring(Action callbackAction)
        {
            Debug.Assert(string.IsNullOrWhiteSpace(buttonName),
                $"{nameof(InputButtonMonitor)} requires a valid Input Manager button name.");

            EndMonitoring();
            this.callbackAction = callbackAction;
            routine = CoroutineRunner.StartManagedCoroutine(WaitForInput());
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            if (routine == null)
            {
                callbackAction = null;
                return;
            }

            CoroutineRunner.StopManagedCoroutine(routine);
            routine = null;
            callbackAction = null;
        }

        private IEnumerator WaitForInput()
        {
            while (true)
            {
                yield return null;

                if (IsTriggered())
                {
                    NotifyCallbackAction();
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
        
        private void NotifyCallbackAction()
        {
            var action = callbackAction;
            callbackAction = null;
            action?.Invoke();
        }
    }
}