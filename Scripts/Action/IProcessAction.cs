using System;

namespace Jungle.Actions
{
    /// <summary>
    /// Describes an action that can run over time and be monitored for completion.
    /// </summary>
    public interface IProcessAction : IImmediateAction,IStateAction
    {
        /// <summary>
        /// Begins running the process using the current configuration.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the process before it completes naturally.
        /// </summary>
        void Interrupt();

        /// <summary>
        /// Raised when the process reports it has finished.
        /// </summary>
        public event Action OnProcessCompleted;

        /// <summary>
        /// Executes the process immediately without waiting for a completion callback.
        /// </summary>
        void IImmediateAction.Execute()
        {
            //start with no call back on completion
            Start();
        }

        /// <summary>
        /// True when the action can report a finite duration.
        /// </summary>
        bool HasDefinedDuration { get; }

        /// <summary>
        /// Gets the expected duration in seconds when available.
        /// </summary>
        float Duration { get; }


        /// <summary>
        /// Indicates whether the process is currently active.
        /// </summary>
        bool IsInProgress { get; }

        /// <summary>
        /// Indicates whether the process has finished executing.
        /// </summary>
        bool HasCompleted { get; }

        void IStateAction.OnStateEnter() => Start();


        void IStateAction.OnStateExit() => Interrupt();

    }


}