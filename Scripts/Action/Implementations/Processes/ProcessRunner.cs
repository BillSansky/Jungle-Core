using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    /// <summary>
    /// Coordinates long-running process actions, advancing them and handling cancellation.
    /// </summary>
    public class ProcessRunner : MonoBehaviour
    {
        /// <summary>
        /// Enumerates the ProcessTiming values.
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

        [JungleClassSelection(typeof(IProcessAction))] [SerializeReference]
        public IProcessAction Process;

        [SerializeField] private ProcessTiming startEvent = ProcessTiming.OnEnable;

        [SerializeField] private ProcessTiming interruptEvent = ProcessTiming.OnDisable;

        [SerializeField]
        private UnityEvent onProcessCompleted;

        public bool IsRunning { get; private set; }
        public bool IsComplete { get; private set; }

        public UnityEvent OnProcessCompleted => onProcessCompleted;
        /// <summary>
        /// Starts or interrupts the configured process when the object awakens, depending on the timing settings.
        /// </summary>
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
        /// <summary>
        /// Handles the OnEnable event.
        /// </summary>
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
        /// <summary>
        /// Triggers start or interrupt behavior during Unity's Start callback based on the selected timing options.
        /// </summary>
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
        /// <summary>
        /// Handles the OnDisable event.
        /// </summary>
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
        /// <summary>
        /// Handles the OnDestroy event.
        /// </summary>
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
        /// <summary>
        /// Handles process completion by updating state, invoking listeners, and removing the subscription.
        /// </summary>
        private void NotifyProcessCompleted()
        {
            IsRunning = false;
            IsComplete = true;
            OnProcessCompleted?.Invoke();
            Process.OnProcessCompleted -= NotifyProcessCompleted;
        }
        /// <summary>
        /// Manually begins the process, tracking running state and subscribing for completion notification.
        /// </summary>
        public void StartProcess()
        {
            IsRunning = true;
            IsComplete = false;
            Process.Start();
            Process.OnProcessCompleted += NotifyProcessCompleted;
        }
        /// <summary>
        /// Stops the active process and marks the runner as no longer running.
        /// </summary>
        public void InterruptProcess()
        {
            IsRunning = false;
            Process.Interrupt();
        }
    }
}