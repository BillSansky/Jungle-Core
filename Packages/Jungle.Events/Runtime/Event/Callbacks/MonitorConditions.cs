using System;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Provides context to an <see cref="IMonitorCondition"/> when evaluating if monitoring should stop.
    /// </summary>
    public readonly struct MonitorConditionContext
    {
        public MonitorConditionContext(int callbackCount, TimeSpan elapsedTime)
        {
            CallbackCount = callbackCount;
            ElapsedTime = elapsedTime;
        }

        /// <summary>
        /// Number of callback invocations observed since monitoring began.
        /// </summary>
        public int CallbackCount { get; }

        /// <summary>
        /// Total elapsed time since monitoring began.
        /// </summary>
        public TimeSpan ElapsedTime { get; }

        /// <summary>
        /// Convenience accessor returning the elapsed time in seconds.
        /// </summary>
        public float ElapsedSeconds => (float)ElapsedTime.TotalSeconds;
    }

    /// <summary>
    /// Describes a condition that determines when an <see cref="IEventMonitor"/> should stop observing.
    /// </summary>
    public interface IMonitorCondition
    {
        /// <summary>
        /// Notifies the condition that monitoring has started.
        /// </summary>
        void OnMonitoringStarted();

        /// <summary>
        /// Returns true when the owning monitor should stop monitoring.
        /// </summary>
        bool ShouldStopMonitoring(MonitorConditionContext context);

        /// <summary>
        /// Notifies the condition that monitoring has ended.
        /// </summary>
        void OnMonitoringEnded();
    }

    /// <summary>
    /// Default condition that never requests monitoring to stop.
    /// </summary>
    [Serializable]
    public sealed class NeverStopMonitorCondition : IMonitorCondition
    {
        public void OnMonitoringStarted()
        {
        }

        public bool ShouldStopMonitoring(MonitorConditionContext context)
        {
            return false;
        }

        public void OnMonitoringEnded()
        {
        }
    }

    /// <summary>
    /// Stops monitoring after a single callback has been invoked.
    /// </summary>
    [Serializable]
    public sealed class SingleCallbackMonitorCondition : IMonitorCondition
    {
        public void OnMonitoringStarted()
        {
        }

        public bool ShouldStopMonitoring(MonitorConditionContext context)
        {
            return context.CallbackCount >= 1;
        }

        public void OnMonitoringEnded()
        {
        }
    }

    /// <summary>
    /// Stops monitoring after a configurable number of callbacks have occurred.
    /// </summary>
    [Serializable]
    public sealed class CallCountMonitorCondition : IMonitorCondition
    {
        [SerializeField]
        [Min(1)]
        private int maximumCallbacks = 3;

        public int MaximumCallbacks
        {
            get => Mathf.Max(1, maximumCallbacks);
            set => maximumCallbacks = Mathf.Max(1, value);
        }

        public void OnMonitoringStarted()
        {
        }

        public bool ShouldStopMonitoring(MonitorConditionContext context)
        {
            return context.CallbackCount >= MaximumCallbacks;
        }

        public void OnMonitoringEnded()
        {
        }
    }

    /// <summary>
    /// Stops monitoring once a specific duration has elapsed.
    /// </summary>
    [Serializable]
    public sealed class TimeElapsedMonitorCondition : IMonitorCondition
    {
        [SerializeField]
        [Min(0f)]
        private float durationSeconds = 1f;

        public float DurationSeconds
        {
            get => Mathf.Max(0f, durationSeconds);
            set => durationSeconds = Mathf.Max(0f, value);
        }

        public void OnMonitoringStarted()
        {
        }

        public bool ShouldStopMonitoring(MonitorConditionContext context)
        {
            return context.ElapsedSeconds >= DurationSeconds;
        }

        public void OnMonitoringEnded()
        {
        }
    }
}
