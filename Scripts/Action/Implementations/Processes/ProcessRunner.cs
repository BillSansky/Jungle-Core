using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    public class ProcessRunner : MonoBehaviour
    {
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
            Process.OnProcessCompleted -= NotifyProcessCompleted;
        }

        public void StartProcess()
        {
            IsRunning = true;
            IsComplete = false;
            Process.Start();
            Process.OnProcessCompleted += NotifyProcessCompleted;
        }

        public void InterruptProcess()
        {
            IsRunning = false;
            Process.Interrupt();
        }
    }
}