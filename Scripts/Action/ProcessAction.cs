using System;

namespace Jungle.Actions
{
    [Serializable]
    public abstract class ProcessAction
    {

        private bool isInProgress;
        private bool isComplete;

        public event Action ProcessStarted;
        public event Action ProcessCompleted;
        public event Action ProcessCancelled;
        
        public bool IsInProgress => isInProgress;
        public bool IsComplete => isComplete;

        /// <summary>
        /// Does this process have a specific duration?
        /// </summary>
        public abstract bool IsTimed { get; }

        public abstract float Duration { get; }


        protected abstract void BeginImpl();


        protected abstract void CompleteImpl();
        
        
        protected virtual void CancelImpl()
        {
            CompleteImpl();
        }


        public void Begin()
        {
            Cancel();

            isInProgress = true;
            isComplete = false;
            BeginImpl();
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
            ProcessCancelled?.Invoke();
        }

        public void Complete()
        {
            isInProgress = false;
            isComplete = true;
            CompleteImpl();
            ProcessCompleted?.Invoke();
        }

    

     
    }
}
