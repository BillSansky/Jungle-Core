using System;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    public class LoopStrategy : ILoopStrategy
    {
        [SerializeField] private int maxIterations = -1;

        public bool ShouldContinue(int completedIterations)
        {
            return maxIterations < 0 || completedIterations < maxIterations;
        }

        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            return normalizedTime % 1f;
        }

        public void Reset()
        {
        }
    }
}
