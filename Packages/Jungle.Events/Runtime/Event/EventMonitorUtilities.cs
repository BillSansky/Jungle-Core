using System;

namespace Jungle.Events
{
    /// <summary>
    /// Helper methods for working with <see cref="IEventMonitor"/> implementations.
    /// </summary>
    public static class EventMonitorUtilities
    {
        /// <summary>
        /// Starts monitoring using the provided <paramref name="monitor"/> and <paramref name="callbackAction"/>.
        /// </summary>
        /// <param name="monitor">Monitor to start.</param>
        /// <param name="callbackAction">Callback action to invoke once monitoring completes.</param>
        public static void StartMonitoring(IEventMonitor monitor, Action callbackAction)
        {
            if (monitor == null)
            {
                throw new ArgumentNullException(nameof(monitor));
            }

            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

            monitor.StartMonitoring(callbackAction);
        }

        /// <summary>
        /// Requests the provided <paramref name="monitor"/> to end monitoring and clear its callback.
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
    }
}
