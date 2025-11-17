using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Waits until a <see cref="EventAsset"/> raises before notifying callback actions.
    /// </summary>
    [Serializable]
    public sealed class EventMonitor : IEventMonitor
    {
        [SerializeField]
        private EventAsset eventAsset;

        [SerializeReference]
        [JungleClassSelection(typeof(IMonitorCondition))]
        private IMonitorCondition monitorCondition = new NeverStopMonitorCondition();

        private readonly MonitorConditionEvaluator monitorConditionEvaluator = new();
        private Action monitoredCallback;
        private bool isListening;

        /// <inheritdoc />
        public IMonitorCondition MonitorCondition
        {
            get => monitorCondition;
            set => monitorCondition = value ?? new NeverStopMonitorCondition();
        }
        

        /// <inheritdoc />
        public void StartMonitoring(Action callbackAction)
        {
            
            StopListening();
            monitoredCallback = monitorConditionEvaluator.CreateMonitoredCallback(callbackAction, EndMonitoring,
                monitorCondition);
            eventAsset.Register(OnEventRaised);
            isListening = true;
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            StopListening();
            monitorConditionEvaluator.Reset();
            monitoredCallback = null;
        }

        private void StopListening()
        {
            if (!isListening)
            {
                return;
            }
            
            eventAsset.Unregister(OnEventRaised);

            isListening = false;
        }

        private void OnEventRaised()
        {
            monitoredCallback?.Invoke();
        }
    }
}
