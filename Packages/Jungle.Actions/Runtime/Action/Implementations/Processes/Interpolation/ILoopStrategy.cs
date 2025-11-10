using System;

namespace Jungle.Actions
{
    /// <summary>
    /// Defines the loop strategy interface.
    /// </summary>

    public interface ILoopStrategy
    {
        bool ShouldContinue(int completedIterations);
        float GetCurrentTime(float normalizedTime, int currentIteration);
        void Reset();
    }
}
