using System;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Timing
{
    /// <summary>
    /// Counts up time and exposes helpers for delays, looping, and completion checks.
    /// </summary>
    public class Timer : MonoBehaviour, Jungle.Actions.IProcessAction
    {
        [SerializeField]
        [Min(0f)]
        private float duration = 1f;

        [SerializeField]
        private bool waitForOneFrame;

        [SerializeField]
        private bool useUnscaledTime;

        [SerializeField]
        private UnityEvent onStarted = new UnityEvent();

        [SerializeField]
        private UnityEvent onCompleted = new UnityEvent();

        private float remainingTime;
        private bool isRunning;
        private bool isPaused;
        private bool isWaitingForFrame;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public float Duration
        {
            get => duration;
            set
            {
                duration = Mathf.Max(0f, value);
                if (!isRunning)
                {
                    remainingTime = duration;
                }
            }
        }

        public bool WaitForOneFrame
        {
            get => waitForOneFrame;
            set => waitForOneFrame = value;
        }

        public bool UseUnscaledTime
        {
            get => useUnscaledTime;
            set => useUnscaledTime = value;
        }

        public bool IsRunning => isRunning;

        public bool IsPaused => isPaused;

        public float RemainingTime => remainingTime;

        public UnityEvent Started => onStarted;

        public UnityEvent Completed => onCompleted;

        // IProcessAction implementation
        public bool HasDefinedDuration => !float.IsInfinity(duration);

        public bool IsInProgress => isRunning;

        public bool HasCompleted => hasCompleted;
        /// <summary>
        /// Begins the timer using the configured duration.
        /// </summary>
        public void Start()
        {
            StartTimer();
        }
        /// <summary>
        /// Stops the timer without firing completion callbacks.
        /// </summary>
        public void Interrupt()
        {
            StopTimer();
        }
        /// <summary>
        /// Starts the timer using its current duration value.
        /// </summary>
        public void StartTimer()
        {
            StartTimer(duration);
        }
        /// <summary>
        /// Starts the timer with a custom duration, resetting all tracking flags.
        /// </summary>
        public void StartTimer(float customDuration)
        {
            duration = Mathf.Max(0f, customDuration);
            remainingTime = duration;
            isRunning = true;
            isPaused = false;
            isWaitingForFrame = waitForOneFrame;
            hasCompleted = false;
            onStarted.Invoke();
        }
        /// <summary>
        /// Stops the timer and clears its running state.
        /// </summary>
        public void StopTimer()
        {
            if (!isRunning)
            {
                return;
            }

            isRunning = false;
            isPaused = false;
            isWaitingForFrame = false;
        }
        /// <summary>
        /// Temporarily pauses countdown progress while keeping state intact.
        /// </summary>
        public void Pause()
        {
            if (isRunning)
            {
                isPaused = true;
            }
        }
        /// <summary>
        /// Resumes countdown progress after a pause.
        /// </summary>
        public void Resume()
        {
            if (isRunning)
            {
                isPaused = false;
            }
        }
        /// <summary>
        /// Advances the timer each frame and triggers completion when the duration elapses.
        /// </summary>
        private void Update()
        {
            if (!isRunning || isPaused)
            {
                return;
            }

            if (isWaitingForFrame)
            {
                isWaitingForFrame = false;
                Complete();
                return;
            }

            if (remainingTime <= 0f)
            {
                Complete();
                return;
            }

            remainingTime -= useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

            if (remainingTime <= 0f)
            {
                Complete();
            }
        }
        /// <summary>
        /// Finalizes the timer run, fires events, and notifies listeners.
        /// </summary>
        private void Complete()
        {
            isRunning = false;
            isPaused = false;
            isWaitingForFrame = false;
            hasCompleted = true;
            onCompleted.Invoke();
            OnProcessCompleted?.Invoke();
        }
    }
}
