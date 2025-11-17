using System;
using System.Diagnostics;

namespace Jungle.Events
{
    /// <summary>
    /// Wraps callback actions so <see cref="IMonitorCondition"/> instances can automatically terminate monitoring.
    /// </summary>
    internal sealed class MonitorConditionEvaluator
    {
        private readonly Stopwatch stopwatch = new();
        private Action callbackAction;
        private Action stopMonitoringAction;
        private IMonitorCondition monitorCondition;
        private int callbackCount;

        public Action CreateMonitoredCallback(Action callback, Action stopMonitoring, IMonitorCondition condition)
        {
            if (callback == null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            if (stopMonitoring == null)
            {
                throw new ArgumentNullException(nameof(stopMonitoring));
            }

            Reset();

            callbackAction = callback;
            stopMonitoringAction = stopMonitoring;
            monitorCondition = condition ?? new NeverStopMonitorCondition();
            callbackCount = 0;
            stopwatch.Restart();
            monitorCondition.OnMonitoringStarted();

            return Invoke;
        }

        public void Reset()
        {
            stopwatch.Reset();
            monitorCondition?.OnMonitoringEnded();
            monitorCondition = null;
            callbackAction = null;
            stopMonitoringAction = null;
            callbackCount = 0;
        }

        private void Invoke()
        {
            callbackAction?.Invoke();

            if (monitorCondition == null)
            {
                return;
            }

            callbackCount++;
            var context = new MonitorConditionContext(callbackCount, stopwatch.Elapsed);
            if (monitorCondition.ShouldStopMonitoring(context))
            {
                stopMonitoringAction?.Invoke();
            }
        }
    }
}
