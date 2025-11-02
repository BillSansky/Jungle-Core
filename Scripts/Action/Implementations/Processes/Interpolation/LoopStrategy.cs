using System;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Repeats interpolation for a fixed number of iterations.
    /// </summary>
    [Serializable]
    public class LoopStrategy : ILoopStrategy
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
        /// Calculates the normalized time for the current iteration.
        /// </summary>

        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            return normalizedTime % 1f;
        }
        /// <summary>
        /// Resets any internal loop state.
        /// </summary>

        public void Reset()
        {
        }
    }
}
