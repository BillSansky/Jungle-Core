using System;

namespace Jungle.Actions
{

    public interface ILoopStrategy
    {
        bool ShouldContinue(int completedIterations);
        float GetCurrentTime(float normalizedTime, int currentIteration);
        void Reset();
    }
}
