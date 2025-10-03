using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Timing
{
    public class Timer : MonoBehaviour
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

        public void StartTimer()
        {
            StartTimer(duration);
        }

        public void StartTimer(float customDuration)
        {
            duration = Mathf.Max(0f, customDuration);
            remainingTime = duration;
            isRunning = true;
            isPaused = false;
            isWaitingForFrame = waitForOneFrame;
            onStarted.Invoke();
        }

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

        public void Pause()
        {
            if (isRunning)
            {
                isPaused = true;
            }
        }

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
            onCompleted.Invoke();
        }
    }
}
