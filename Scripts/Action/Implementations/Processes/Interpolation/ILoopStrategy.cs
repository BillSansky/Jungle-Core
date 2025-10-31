using System;

namespace Jungle.Actions
{
    /// <summary>
    /// Defines the ILoopStrategy contract.
    /// </summary>
    public interface ILoopStrategy
    {
        /// <summary>
        /// Indicates whether the process should run another loop after completing the previous iteration.
        /// </summary>
        bool ShouldContinue(int completedIterations);
        /// <summary>
        /// Adjusts the normalized timeline position to account for the current loop iteration.
        /// </summary>
        float GetCurrentTime(float normalizedTime, int currentIteration);
        /// <summary>
        /// Clears any internal state to prepare for a fresh run.
        /// </summary>
        void Reset();
    }
}
