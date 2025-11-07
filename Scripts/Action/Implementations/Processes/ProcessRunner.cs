using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    /// <summary>
    /// MonoBehaviour helper that starts and interrupts an <see cref="IProcessAction"/> based on Unity callbacks.
    /// </summary>
    public class ProcessRunner : MonoBehaviour
    {
        /// <summary>
        /// Unity callback moments that can start or interrupt the process.
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

        [SerializeField] private ProcessTiming interruptEvent = ProcessTiming.OnDisable;

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

        private void Awake()
        {
            if (startEvent == ProcessTiming.Awake)
            {
                StartProcess();
            }

            if (interruptEvent == ProcessTiming.Awake)
            {
                InterruptProcess();
            }
        }

        private void OnEnable()
        {
            if (startEvent == ProcessTiming.OnEnable)
            {
                StartProcess();
            }

            if (interruptEvent == ProcessTiming.OnEnable)
            {
                InterruptProcess();
            }
        }

        private void Start()
        {
            if (startEvent == ProcessTiming.Start)
            {
                StartProcess();
            }

            if (interruptEvent == ProcessTiming.Start)
            {
                InterruptProcess();
            }
        }

        private void OnDisable()
        {
            if (startEvent == ProcessTiming.OnDisable)
            {
                StartProcess();
            }

            if (interruptEvent == ProcessTiming.OnDisable)
            {
                InterruptProcess();
            }
        }

        private void OnDestroy()
        {
            if (startEvent == ProcessTiming.OnDestroy)
            {
                StartProcess();
            }

            if (interruptEvent == ProcessTiming.OnDestroy)
            {
                InterruptProcess();
            }
        }

        private void NotifyProcessCompleted()
        {
            IsRunning = false;
            IsComplete = true;
            OnProcessCompleted?.Invoke();
        }

        /// <summary>
        /// Starts the configured process and wires completion callbacks.
        /// </summary>
        public void StartProcess()
        {
            IsRunning = true;
            IsComplete = false;
            Process.Start(null);
        }

        /// <summary>
        /// Interrupts the configured process.
        /// </summary>
        public void InterruptProcess()
        {
            IsRunning = false;
            Process.Interrupt();
        }
    }
}