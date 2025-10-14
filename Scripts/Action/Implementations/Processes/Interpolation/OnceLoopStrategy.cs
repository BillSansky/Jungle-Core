using System;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    public class OnceLoopStrategy : ILoopStrategy
    {
        public bool ShouldContinue(int completedIterations)
        {
            return completedIterations < 1;
        }

        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            return Mathf.Clamp01(normalizedTime);
        }

        public void Reset()
        {
        }
    }
}
