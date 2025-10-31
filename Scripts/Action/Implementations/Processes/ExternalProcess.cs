using System;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// An IProcessAction implementation that runs a process via an external ProcessRunner component.
    /// This allows ProcessRunner instances to be integrated into the Jungle Actions framework.
    /// </summary>
    [Serializable]
    public class ExternalProcess : IProcessAction
    {
        [Tooltip("Reference to the ProcessRunner component that will execute the process.")]
        [SerializeField] 
        private ProcessRunner processRunner;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => false;

        public float Duration => -1f;

        public bool IsInProgress => processRunner != null && processRunner.IsRunning;

        public bool HasCompleted => processRunner != null && processRunner.IsComplete;
        /// <summary>
        /// Starts the referenced runner and hooks completion so the wrapper can relay notifications.
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
        /// Cancels the external runner if it is active and detaches event listeners.
        /// </summary>
        public void Interrupt()
        {
            if (processRunner != null && processRunner.IsRunning)
            {
                processRunner.InterruptProcess();
                Cleanup();
            }
        }
        /// <summary>
        /// Relays the completion event from the runner and performs cleanup.
        /// </summary>
        private void HandleProcessCompleted()
        {
            OnProcessCompleted?.Invoke();
            Cleanup();
        }
        /// <summary>
        /// Removes the completion listener from the referenced runner.
        /// </summary>
        private void Cleanup()
        {
            if (processRunner != null)
            {
                processRunner.OnProcessCompleted.RemoveListener(HandleProcessCompleted);
            }
        }
    }
}
