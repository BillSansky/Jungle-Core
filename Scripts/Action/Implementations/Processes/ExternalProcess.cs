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

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => false;

        public float Duration => -1f;

        public bool IsInProgress => processRunner != null && processRunner.IsRunning;

        public bool HasCompleted => processRunner != null && processRunner.IsComplete;

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
