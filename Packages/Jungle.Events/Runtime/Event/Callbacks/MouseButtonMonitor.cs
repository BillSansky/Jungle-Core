using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Waits for a mouse button interaction before notifying callback actions.
    /// </summary>
    [Serializable]
    public sealed class MouseButtonMonitor : IEventMonitor
    {
        [SerializeField]
        [Tooltip("Index of the mouse button to monitor. 0 = Left, 1 = Right, 2 = Middle.")]
        private int mouseButtonIndex;

        [SerializeField]
        private InputInteraction interaction = InputInteraction.ButtonDown;

        [SerializeReference]
        [JungleClassSelection(typeof(IMonitorCondition))]
        private IMonitorCondition monitorCondition = new NeverStopMonitorCondition();

        private readonly MonitorConditionEvaluator monitorConditionEvaluator = new();
        private Action monitoredCallback;
        private Coroutine routine;

        /// <inheritdoc />
        public IMonitorCondition MonitorCondition
        {
            get => monitorCondition;
            set => monitorCondition = value ?? new NeverStopMonitorCondition();
        }

        /// <inheritdoc />
        public void StartMonitoring(Action callbackAction)
        {
            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

            EnsureValidButtonIndex();

            EndMonitoring();
            monitoredCallback = monitorConditionEvaluator.CreateMonitoredCallback(callbackAction, EndMonitoring,
                monitorCondition);
            routine = CoroutineRunner.StartManagedCoroutine(WaitForMouse());
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            if (routine != null)
            {
                CoroutineRunner.StopManagedCoroutine(routine);
                routine = null;
            }

            monitorConditionEvaluator.Reset();
            monitoredCallback = null;
        }

        private IEnumerator WaitForMouse()
        {
            while (true)
            {
                yield return null;

                if (IsTriggered())
                {
                    NotifyCallbackAction();
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
                    $"{nameof(MouseButtonMonitor)} expects a button index between 0 and 6. Value {mouseButtonIndex} is outside the supported range.");
            }
        }

        private void NotifyCallbackAction()
        {
            monitoredCallback?.Invoke();
        }
    }
}
