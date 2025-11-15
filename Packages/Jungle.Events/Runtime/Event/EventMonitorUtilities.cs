using System;

namespace Jungle.Events
{
    /// <summary>
    /// Helper methods for working with <see cref="IEventMonitor"/> implementations.
    /// </summary>
    public static class EventMonitorUtilities
    {
        /// <summary>
        /// Attaches the provided <paramref name="callbackAction"/> to the <paramref name="monitor"/>,
        /// starts monitoring immediately, and returns a disposable handle that can be used to
        /// unsubscribe and stop monitoring.
        /// </summary>
        /// <param name="monitor">Monitor to subscribe to.</param>
        /// <param name="callbackAction">Action to invoke when the monitor signals completion.</param>
        /// <returns>A disposable subscription handle.</returns>
        public static EventMonitorSubscription Subscribe(IEventMonitor monitor, Action callbackAction)
        {
            if (monitor == null)
            {
                throw new ArgumentNullException(nameof(monitor));
            }

            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

            return new EventMonitorSubscription(monitor, callbackAction);
        }

        /// <summary>
        /// Ensures the provided <paramref name="monitor"/> is actively monitoring.
        /// </summary>
        /// <param name="monitor">Monitor to start.</param>
        public static void EnsureMonitoring(IEventMonitor monitor)
        {
            if (monitor == null)
            {
                throw new ArgumentNullException(nameof(monitor));
            }

            monitor.StartMonitoring();
        }

        /// <summary>
        /// Requests the provided <paramref name="monitor"/> to end monitoring.
        /// </summary>
        /// <param name="monitor">Monitor to stop.</param>
        public static void StopMonitoring(IEventMonitor monitor)
        {
            if (monitor == null)
            {
                throw new ArgumentNullException(nameof(monitor));
            }

            monitor.EndMonitoring();
        }

        /// <summary>
        /// Restarts monitoring using the provided <paramref name="subscription"/> handle.
        /// </summary>
        /// <param name="subscription">Subscription to restart.</param>
        public static void Restart(EventMonitorSubscription subscription)
        {
            if (subscription == null)
            {
                throw new ArgumentNullException(nameof(subscription));
            }

            subscription.Restart();
        }
    }
}
