using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Executes registered callback actions after a configurable delay.
    /// </summary>
    [Serializable]
    public sealed class TimeCallback : IEventMonitor
    {
        [SerializeReference]
        [JungleClassSelection(typeof(IFloatValue))]
        private IFloatValue delay = new FloatValue(1f);

        [SerializeField]
        private bool useUnscaledTime;

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

            EndMonitoring();
            monitoredCallback = monitorConditionEvaluator.CreateMonitoredCallback(callbackAction, EndMonitoring,
                monitorCondition);
            routine = CoroutineRunner.StartManagedCoroutine(WaitRoutine());
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

        private IEnumerator WaitRoutine()
        {
            var waitDuration = Mathf.Max(0f, delay?.Value() ?? 0f);

            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(waitDuration);
            }
            else
            {
                yield return new WaitForSeconds(waitDuration);
            }

            NotifyCallbackAction();
            routine = null;
        }

        private void NotifyCallbackAction()
        {
            monitoredCallback?.Invoke();
        }
    }
}
