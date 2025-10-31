using System;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Repeats the interpolation by reversing direction every iteration.
    /// </summary>
    [Serializable]
    public class PingPongLoopStrategy : ILoopStrategy
    {
        /// <summary>
        /// Keeps looping until the configured iteration limit is reached (or runs forever when negative).
        /// </summary>
        [SerializeField] private int maxIterations = -1;

        public bool ShouldContinue(int completedIterations)
        {
            return maxIterations < 0 || completedIterations < maxIterations;
        }
        /// <summary>
        /// Reflects the normalized time back and forth between 0 and 1 to produce a ping-pong motion.
        /// </summary>
        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            var pingPongTime = Mathf.PingPong(normalizedTime, 1f);
            return pingPongTime;
        }
        /// <summary>
        /// Clears any stored state; this strategy does not maintain additional data.
        /// </summary>
        public void Reset()
        {
        }
    }
}
