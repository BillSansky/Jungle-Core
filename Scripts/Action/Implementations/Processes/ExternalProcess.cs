using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// An IProcessAction implementation that runs a process via an external ProcessRunner component.
    /// This allows ProcessRunner instances to be integrated into the Jungle Actions framework.
    /// </summary>
    [Serializable]
    [JungleClassInfo("External Process Action", "Wraps a ProcessRunner component so it can run as a Jungle process.", null, "Actions/Process")]
    public class ExternalProcess : IProcessAction
    {
        [Tooltip("Reference to the ProcessRunner component that will execute the process.")]
        [SerializeField] 
        private ProcessRunner processRunner;
        /// <summary>
        /// Invoked when the process action finishes.
        /// </summary>

        public event Action OnProcessCompleted;
        /// <summary>
        /// Indicates whether the action can report a finite duration.
        /// </summary>

        public bool HasDefinedDuration => false;
        /// <summary>
        /// Gets the total duration of the action in seconds.
        /// </summary>

        public float Duration => -1f;
        /// <summary>
        /// Gets whether the action is currently running.
        /// </summary>

        public bool IsInProgress => processRunner != null && processRunner.IsRunning;
        /// <summary>
        /// Gets whether the action has finished executing.
        /// </summary>

        public bool HasCompleted => processRunner != null && processRunner.IsComplete;
        /// <summary>
        /// Starts the external process.
        /// </summary>

        public void Start()
        {
            Debug.Assert(processRunner != null, "ProcessRunner reference is null");

            if (processRunner == null)
            {
                return;
            }

            // Subscribe to completion event before starting
            processRunner.OnProcessCompleted.AddListener(HandleProcessCompleted);

            // Start the process runner manually
            processRunner.StartProcess();
        }
        /// <summary>
        /// Interrupts the external process before completion.
        /// </summary>

        public void Interrupt()
        {
            if (processRunner != null && processRunner.IsRunning)
            {
                processRunner.InterruptProcess();
                Cleanup();
            }
        }

        private void HandleProcessCompleted()
        {
            OnProcessCompleted?.Invoke();
            Cleanup();
        }

        private void Cleanup()
        {
            if (processRunner != null)
            {
                processRunner.OnProcessCompleted.RemoveListener(HandleProcessCompleted);
            }
        }
    }
}
