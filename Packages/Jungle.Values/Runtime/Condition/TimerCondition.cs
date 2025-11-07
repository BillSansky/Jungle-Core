using System;
using Jungle.Attributes;
using Jungle.Timing;
using UnityEngine;

namespace Jungle.Conditions
{
    /// <summary>
    /// Evaluates a <see cref="Timer"/> and reports whether it matches a selected state.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Timer Condition", "Evaluates the state of a Jungle timer instance.", null, "Timing")]
    public class TimerCondition : Condition
    {
        [SerializeField]
        private Timer timer;

        [SerializeField]
        private TimerState expectedState = TimerState.Running;

        /// <summary>
        /// Checks the timer against the configured state.
        /// </summary>
        protected internal override bool IsValidImpl()
        {
            if (timer == null)
            {
                throw new InvalidOperationException("Timer reference has not been assigned on TimerCondition.");
            }

            return expectedState switch
            {
                TimerState.Started => timer.IsRunning,
                TimerState.Running => timer.IsRunning && !timer.IsPaused,
                TimerState.Paused => timer.IsRunning && timer.IsPaused,
                TimerState.Completed => !timer.IsRunning && timer.RemainingTime <= 0f,
                TimerState.Stopped => !timer.IsRunning,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        /// States that can be matched against the observed timer.
        /// </summary>
        private enum TimerState
        {
            Started,
            Running,
            Paused,
            Completed,
            Stopped,
        }
    }
}
