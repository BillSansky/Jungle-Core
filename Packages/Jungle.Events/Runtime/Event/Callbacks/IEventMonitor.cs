using System;

namespace Jungle.Events
{
    /// <summary>
    /// Represents an event monitor capable of notifying subscribers once its condition is met.
    /// Implementations can watch for timers, input signals, ScriptableObject events and more.
    /// Additional variations such as animation events, physics triggers, timeline markers or network messages can be
    /// implemented by following the same pattern.
    /// </summary>
    public interface IEventMonitor
    {
        /// <summary>
        /// Configurable condition that determines when the monitor should stop automatically.
        /// </summary>
        IMonitorCondition MonitorCondition { get; set; }

        /// <summary>
        /// Begins observing the monitored condition and prepares it to signal the provided callback.
        /// Implementations should replace any previously supplied callback and be idempotent to allow
        /// multiple start requests.
        /// </summary>
        /// <param name="callbackAction">Callback action to invoke when the monitor signals completion.</param>
        void StartMonitoring(Action callbackAction);

        /// <summary>
        /// Stops observing the monitored condition, releases any resources in use, and removes the current callback.
        /// Implementations should be idempotent to allow multiple stop requests.
        /// </summary>
        void EndMonitoring();
    }
}
