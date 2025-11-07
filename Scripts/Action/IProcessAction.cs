using System;

namespace Jungle.Actions
{
    /// <summary>
    /// Describes an action that can run over time and be monitored for completion.
    /// </summary>
    public interface IProcessAction
    {
        /// <summary>
        /// Begins running the process using the current configuration.
        /// </summary>
        /// <param name="callback">Optional action invoked when the process completes.</param>
        void Start(Action callback = null);

        /// <summary>
        /// Stops the process before it completes naturally.
        /// </summary>
        void Interrupt();


        bool IsInstant => Duration <= 0;
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

        /// <summary>
        /// Raised when the process finishes executing.
        /// </summary>
        event Action OnProcessCompleted;
    }


}