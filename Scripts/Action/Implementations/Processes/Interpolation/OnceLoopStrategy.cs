using System;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Runs the interpolation a single time with clamped progress.
    /// </summary>
    [Serializable]
    public class OnceLoopStrategy : ILoopStrategy
    {
        /// <summary>
        /// Determines whether another iteration should run.
        /// </summary>
        public bool ShouldContinue(int completedIterations)
        {
            return completedIterations < 1;
        }
        /// <summary>
        /// Clamps the normalized time to the [0,1] range.
        /// </summary>

        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            return Mathf.Clamp01(normalizedTime);
        }
        /// <summary>
        /// Resets any internal loop state.
        /// </summary>

        public void Reset()
        {
        }
    }
}
