using System;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Loops interpolation forward and backward using a ping-pong pattern.
    /// </summary>
    [Serializable]
    public class PingPongLoopStrategy : ILoopStrategy
    {
        /// <summary>
        /// Determines whether another iteration should run.
        /// </summary>
        [SerializeField] private int maxIterations = -1;

        public bool ShouldContinue(int completedIterations)
        {
            return maxIterations < 0 || completedIterations < maxIterations;
        }
        /// <summary>
        /// Calculates the interpolated time for the current ping-pong iteration.
        /// </summary>

        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            var pingPongTime = Mathf.PingPong(normalizedTime, 1f);
            return pingPongTime;
        }
        /// <summary>
        /// Resets any internal loop state.
        /// </summary>

        public void Reset()
        {
        }
    }
}
