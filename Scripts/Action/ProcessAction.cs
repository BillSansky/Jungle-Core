using System;

namespace Jungle.Actions
{
    [Serializable]
    public abstract class ProcessAction : IBeginEndAction
    {

        private bool isInProgress;
        private bool isComplete;

        public event Action OnProcessStarted;
        public event Action OnProcessCompleted;
        public event Action OnProcessInterrupted;
        
        public bool IsInProgress => isInProgress;
        public bool IsComplete => isComplete;

        /// <summary>
        /// Does this process have a specific duration?
        /// </summary>
        public abstract bool IsTimed { get; }

        public abstract float Duration { get; }


        protected abstract void BeginImpl();
        protected abstract void InterruptOrCompleteCleanup();

        protected abstract void RegisterInternalCompletionListener( Action onCompleted);
        
        
        public void Begin()
        {
            isInProgress = true;
            isComplete = false;
            BeginImpl();
            RegisterInternalCompletionListener(NotifyComplete);
            OnProcessStarted?.Invoke();
        }
        

        public void End()
        {
            
            Interrupt();
        }
     
        public void Interrupt()
        {
            if (!isInProgress)
                return;
            
            isInProgress = false;
            isComplete = true;
            InterruptOrCompleteCleanup();
            OnProcessInterrupted?.Invoke();
        }
        
        protected void NotifyComplete()
        {
            isInProgress = false;
            isComplete = true;
            InterruptOrCompleteCleanup();
            OnProcessCompleted?.Invoke();
        }
        
    }
}
