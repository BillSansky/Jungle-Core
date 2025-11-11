using System;
using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    /// <summary>
    /// MonoBehaviour helper that starts and stops an <see cref="IProcessAction"/> based on Unity callbacks.
    /// </summary>
    public class ProcessRunner : MonoBehaviour
    {
        /// <summary>
        /// Unity callback moments that can start or stop the process.
        /// </summary>
        public enum ProcessTiming
        {
            Awake,
            OnEnable,
            Start,
            OnDisable,
            OnDestroy,
            Manual
        }
        /// <summary>
        /// Gets or sets the process action to run.
        /// </summary>

        [JungleClassSelection(typeof(IProcessAction))] [SerializeReference]
        public IProcessAction Process;

        [SerializeField] private ProcessTiming startEvent = ProcessTiming.OnEnable;

        [SerializeField] private ProcessTiming stopEvent = ProcessTiming.OnDisable;

        [SerializeField]
        private UnityEvent onProcessCompleted;

        /// <summary>
        /// True while the configured process is running.
        /// </summary>
        public bool IsRunning { get; private set; }
        /// <summary>
        /// True once the configured process reports completion.
        /// </summary>
        public bool IsComplete { get; private set; }

        /// <summary>
        /// UnityEvent invoked when the process completes.
        /// </summary>
        public UnityEvent OnProcessCompleted => onProcessCompleted;

        private Action processCompletionCallback;

        private void Awake()
        {
            if (startEvent == ProcessTiming.Awake)
            {
                StartProcess();
            }

            if (stopEvent == ProcessTiming.Awake)
            {
                StopProcess();
            }
        }

        private void OnEnable()
        {
            if (startEvent == ProcessTiming.OnEnable)
            {
                StartProcess();
            }

            if (stopEvent == ProcessTiming.OnEnable)
            {
                StopProcess();
            }
        }

        private void Start()
        {
            if (startEvent == ProcessTiming.Start)
            {
                StartProcess();
            }

            if (stopEvent == ProcessTiming.Start)
            {
                StopProcess();
            }
        }

        private void OnDisable()
        {
            if (startEvent == ProcessTiming.OnDisable)
            {
                StartProcess();
            }

            if (stopEvent == ProcessTiming.OnDisable)
            {
                StopProcess();
            }
        }

        private void OnDestroy()
        {
            if (startEvent == ProcessTiming.OnDestroy)
            {
                StartProcess();
            }

            if (stopEvent == ProcessTiming.OnDestroy)
            {
                StopProcess();
            }
        }

        private void NotifyProcessCompleted()
        {
            IsRunning = false;
            IsComplete = true;
            OnProcessCompleted?.Invoke();
        }

        private void HandleProcessActionCompleted()
        {
            processCompletionCallback = null;
            NotifyProcessCompleted();
        }

        /// <summary>
        /// Starts the configured process and wires completion callbacks.
        /// </summary>
        public void StartProcess()
        {
            if (Process == null)
            {
                IsRunning = false;
                IsComplete = false;
                return;
            }

            IsRunning = true;
            IsComplete = false;
            processCompletionCallback = HandleProcessActionCompleted;
            Process.StartProcess(processCompletionCallback);
        }

        /// <summary>
        /// Stops the configured process.
        /// </summary>
        public void StopProcess()
        {
            IsRunning = false;
            IsComplete = false;
            processCompletionCallback = null;
            Process?.Stop();
        }
    }
}