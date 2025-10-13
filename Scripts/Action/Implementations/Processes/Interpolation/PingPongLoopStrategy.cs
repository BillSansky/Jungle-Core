using System;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    public class PingPongLoopStrategy : ILoopStrategy
    {
        [SerializeField] private int maxIterations = -1;

        public bool ShouldContinue(int completedIterations)
        {
            return maxIterations < 0 || completedIterations < maxIterations;
        }

        public float GetCurrentTime(float normalizedTime, int currentIteration)
        {
            var pingPongTime = Mathf.PingPong(normalizedTime, 1f);
            return pingPongTime;
        }

        public void Reset()
        {
        }
    }
}
