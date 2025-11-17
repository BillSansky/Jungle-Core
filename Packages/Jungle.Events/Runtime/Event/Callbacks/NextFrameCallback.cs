using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Triggers callback actions after waiting for a single rendered frame.
    /// </summary>
    [Serializable]
    public sealed class NextFrameCallback : IEventMonitor
    {
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

            EndMonitoring();
            monitoredCallback = monitorConditionEvaluator.CreateMonitoredCallback(callbackAction, EndMonitoring,
                monitorCondition);
            routine = CoroutineRunner.StartManagedCoroutine(WaitForFrame());
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

        private IEnumerator WaitForFrame()
        {
            yield return null;
            NotifyCallbackAction();
            routine = null;
        }

        private void NotifyCallbackAction()
        {
            monitoredCallback?.Invoke();
        }
    }
}
