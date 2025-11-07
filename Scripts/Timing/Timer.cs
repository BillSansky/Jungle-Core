using System;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Timing
{
    /// <summary>
    /// Runs a countdown and raises UnityEvents when it starts and completes.
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
        private Action completionCallback;

        public event Action OnProcessCompleted;

        /// <summary>
        /// Gets the total duration of the action in seconds.
        /// </summary>
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

        /// <summary>
        /// When true the timer waits one frame before counting down.
        /// </summary>
        public bool WaitForOneFrame
        {
            get => waitForOneFrame;
            set => waitForOneFrame = value;
        }

        /// <summary>
        /// When true the timer advances using unscaled time.
        /// </summary>
        public bool UseUnscaledTime
        {
            get => useUnscaledTime;
            set => useUnscaledTime = value;
        }

        /// <summary>
        /// Indicates whether the timer is currently running.
        /// </summary>
        public bool IsRunning => isRunning;

        /// <summary>
        /// Indicates whether the timer is paused.
        /// </summary>
        public bool IsPaused => isPaused;

        /// <summary>
        /// Gets the remaining time.
        /// </summary>
        public float RemainingTime => remainingTime;

        /// <summary>
        /// UnityEvent invoked when the timer starts.
        /// </summary>
        public UnityEvent Started => onStarted;

        /// <summary>
        /// UnityEvent invoked when the timer completes.
        /// </summary>
        public UnityEvent Completed => onCompleted;

        // IProcessAction implementation
        /// <inheritdoc />
        public bool HasDefinedDuration => !float.IsInfinity(duration);

        /// <inheritdoc />
        public bool IsInProgress => isRunning;

        /// <inheritdoc />
        public bool HasCompleted => hasCompleted;

        /// <summary>
        /// Starts counting down using the configured duration.
        /// </summary>
        /// <param name="callback"></param>
        public void Start(Action callback = null)
        {
            completionCallback = callback;
            StartTimer();
        }

        /// <summary>
        /// Stops the timer without firing the completion event.
        /// </summary>
        public void Interrupt()
        {
            StopTimer();
        }

        /// <summary>
        /// Starts the timer using the current duration.
        /// </summary>
        public void StartTimer()
        {
            StartTimer(duration);
        }

        /// <summary>
        /// Starts the timer with a custom duration in seconds.
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
        /// Stops the timer immediately.
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
            completionCallback = null;
        }

        /// <summary>
        /// Pauses the timer while preserving the remaining time.
        /// </summary>
        public void Pause()
        {
            if (isRunning)
            {
                isPaused = true;
            }
        }

        /// <summary>
        /// Resumes the timer after it was paused.
        /// </summary>
        public void Resume()
        {
            if (isRunning)
            {
                isPaused = false;
            }
        }

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

        private void Complete()
        {
            isRunning = false;
            isPaused = false;
            isWaitingForFrame = false;
            hasCompleted = true;
            onCompleted.Invoke();
            OnProcessCompleted?.Invoke();
            completionCallback?.Invoke();
            completionCallback = null;
        }
    }
}
