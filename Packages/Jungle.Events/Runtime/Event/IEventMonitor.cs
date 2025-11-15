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
        /// Registers a callback action that will be invoked when the monitor signals completion.
        /// </summary>
        /// <param name="callbackAction">Callback action to register.</param>
        void Attach(Action callbackAction);

        /// <summary>
        /// Removes a callback action previously added via <see cref="Attach"/>.
        /// </summary>
        /// <param name="callbackAction">Callback action to remove.</param>
        void Detach(Action callbackAction);

        /// <summary>
        /// Begins observing the monitored condition and prepares it to signal subscribers.
        /// Implementations should be idempotent to allow multiple start requests.
        /// </summary>
        void StartMonitoring();

        /// <summary>
        /// Stops observing the monitored condition and releases any resources in use.
        /// Implementations should be idempotent to allow multiple stop requests.
        /// </summary>
        void EndMonitoring();
    }
}
