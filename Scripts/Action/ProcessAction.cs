using System;

namespace Jungle.Actions
{
    [Serializable]
    public abstract class ProcessAction : IBeginEndAction
    {

        private bool isInProgress;
        private bool isComplete;

        public event Action ProcessStarted;
        public event Action ProcessCompleted;
        
        public bool IsInProgress => isInProgress;
        public bool IsComplete => isComplete;

        /// <summary>
        /// Does this process have a specific duration?
        /// </summary>
        public abstract bool IsTimed { get; }

        public abstract float Duration { get; }


        protected abstract void BeginImpl();


        protected abstract void CompleteImpl();
        
        
        public void Begin()
        {
            isInProgress = true;
            isComplete = false;
            BeginImpl();
            ProcessStarted?.Invoke();
        }
        

        public void End()
        {
            isInProgress = false;
            isComplete = true;
            CompleteImpl();
            ProcessCompleted?.Invoke();
        }
     
    }
}
