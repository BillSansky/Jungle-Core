using System;

namespace Jungle.Events
{
    /// <summary>
    /// Represents a managed subscription to an <see cref="IEventMonitor"/>.
    /// Disposing the subscription detaches the callback and stops monitoring.
    /// </summary>
    public sealed class EventMonitorSubscription : IDisposable
    {
        private readonly IEventMonitor monitor;
        private readonly Action callbackAction;
        private bool isDisposed;

        internal EventMonitorSubscription(IEventMonitor monitor, Action callbackAction)
        {
            this.monitor = monitor ?? throw new ArgumentNullException(nameof(monitor));
            this.callbackAction = callbackAction ?? throw new ArgumentNullException(nameof(callbackAction));

            this.monitor.Attach(this.callbackAction);
            this.monitor.StartMonitoring();
        }

        /// <summary>
        /// Indicates whether the subscription has been disposed.
        /// </summary>
        public bool IsDisposed => isDisposed;

        /// <summary>
        /// Requests the underlying monitor to restart its observation cycle.
        /// </summary>
        public void Restart()
        {
            if (isDisposed)
            {
                throw new ObjectDisposedException(nameof(EventMonitorSubscription));
            }

            monitor.StartMonitoring();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (isDisposed)
            {
                return;
            }

            monitor.Detach(callbackAction);
            monitor.EndMonitoring();
            isDisposed = true;
        }
    }
}
