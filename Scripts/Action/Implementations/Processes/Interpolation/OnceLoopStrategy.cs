using System;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Plays the interpolation one time and stops when the duration completes.
    /// </summary>
    [Serializable]
    public class OnceLoopStrategy : ILoopStrategy
    {
        /// <summary>
        /// Allows exactly one pass by disallowing any iterations after the first completes.
        /// </summary>
        public bool ShouldContinue(int completedIterations)
        {
            return completedIterations < 1;
        }
        /// <summary>
        /// Clamps the interpolation time to the 0-1 range for the single pass.
        /// </summary>
        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            return Mathf.Clamp01(normalizedTime);
        }
        /// <summary>
        /// No internal state is tracked, so reset is a no-op.
        /// </summary>
        public void Reset()
        {
        }
    }
}
