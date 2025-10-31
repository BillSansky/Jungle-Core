using System;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Base class that implements shared logic for different interpolation loop behaviours.
    /// </summary>
    [Serializable]
    public class LoopStrategy : ILoopStrategy
    {
        /// <summary>
        /// Returns whether another iteration should run based on the configured iteration cap.
        /// </summary>
        [SerializeField] private int maxIterations = -1;

        public bool ShouldContinue(int completedIterations)
        {
            return maxIterations < 0 || completedIterations < maxIterations;
        }
        /// <summary>
        /// Wraps the incoming normalized time into the current loop's 0-1 range.
        /// </summary>
        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            return normalizedTime % 1f;
        }
        /// <summary>
        /// Resets any internal state; the base implementation has nothing to clear.
        /// </summary>
        public void Reset()
        {
        }
    }
}
