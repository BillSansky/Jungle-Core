using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    [Serializable]
    public abstract class ProcessAction
    {

        [SerializeField]
        private UnityEvent onProcessStarted = new UnityEvent();

        [SerializeField]
        private UnityEvent onProcessComplete = new UnityEvent();

        [SerializeField]
        private UnityEvent onProcessFailed = new UnityEvent();
        
        private bool isInProgress;
        private bool isComplete;

        public UnityEvent OnProcessComplete => onProcessComplete;

        public UnityEvent OnProcessFailed => onProcessFailed;

        public UnityEvent OnProcessStarted => onProcessStarted;

        public bool IsInProgress => isInProgress;

        public bool IsComplete => isComplete;

        //Does this process have a specific duration?
       public abstract bool IsTimed { get; }
        
        public void Cancel()
        {
            var wasRunning = isInProgress;
            
            if (!wasRunning)
            {
                return;
            }
        
            isInProgress = false;
            isComplete = false;
            CancelImpl();
            onProcessFailed.Invoke();
        }

        protected abstract void CancelImpl();
        

        public void Complete()
        {

            isInProgress = false;
            isComplete = true;
            
            CompleteImpl();
            
            onProcessComplete.Invoke();
        }
        
        protected abstract void CompleteImpl();

     

      

        public void Begin()
        {

            Cancel();

            isInProgress = true;
            isComplete = false;
            BeginProcessImpl();
            onProcessStarted.Invoke();

        }
        
        protected abstract void BeginProcessImpl();

       
    }
}
