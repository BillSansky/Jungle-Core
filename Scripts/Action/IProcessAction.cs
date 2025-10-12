using System;

namespace Jungle.Actions
{
    /// <summary>
    /// Defines an interface for asynchronous or long-running actions that can be started, interrupted,
    /// and monitored for progress and completion. Extends the IImmediateAction interface to provide
    /// additional functionality related to process management.
    /// </summary>
    public interface IProcessAction : IImmediateAction
    {
        void Start();
        
        void Interrupt();
        
        public event Action OnProcessCompleted;

        /// <summary>
        /// Executes an action immediately without requiring a duration or delay.
        /// Implements the functionality to initiate a process with no callback on completion.
        /// </summary>
        void IImmediateAction.Execute()
        {
            //start with no call back on completion
            Start();
        }
        
        /// <summary>
        /// false if it is infinite, or if the exact duration cannot be determined.
        /// </summary>
        bool HasDefinedDuration { get; }

        /// <summary>
        /// The time span, in seconds, required to complete the process or action.
        /// Returns a precise value when the duration is known; otherwise, behavior depends on implementation.
        /// </summary>
        float Duration { get; }


        /// <summary>
        /// Indicates whether the process is currently ongoing.
        /// </summary>
        bool IsInProgress { get; }

        /// <summary>
        /// Indicates whether the action has finished execution.
        /// </summary>
        bool HasCompleted { get; }
    }
    
    
}