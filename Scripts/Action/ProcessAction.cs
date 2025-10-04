using System;

namespace Jungle.Actions
{
    [Serializable]
    public abstract class ProcessAction
    {
        private bool isStarted;
        private bool isInProgress;
        private bool isComplete;

        public event Action ProcessStarted;
        public event Action ProcessCompleted;
        public event Action ProcessFailed;

        public bool IsStarted => isStarted;
        public bool IsInProgress => isInProgress;
        public bool IsComplete => isComplete;

        /// <summary>
        /// Does this process have a specific duration?
        /// </summary>
        public abstract bool IsTimed { get; }

        public abstract float Duration { get; }
        
        public void Start()
        {
            if (isStarted)
            {
                return;
            }

            isStarted = true;
            OnStart();
        }

        public void Stop()
        {
            if (!isStarted)
            {
                return;
            }

            isStarted = false;
            OnStop();
        }

        protected virtual void OnStart()
        {
        }

        protected virtual void OnStop()
        {
        }

        public void Begin()
        {
            Cancel();

            isInProgress = true;
            isComplete = false;
            BeginProcessImpl();
            ProcessStarted?.Invoke();
        }

        public void Cancel()
        {
            if (!isInProgress)
            {
                return;
            }

            isInProgress = false;
            isComplete = false;
            CancelImpl();
            ProcessFailed?.Invoke();
        }

        public void Complete()
        {
            isInProgress = false;
            isComplete = true;
            CompleteImpl();
            ProcessCompleted?.Invoke();
        }

        protected virtual void BeginProcessImpl()
        {
            Start();
        }

        protected virtual void CancelImpl()
        {
            Stop();
        }

        protected virtual void CompleteImpl()
        {
            Stop();
        }
    }
}
