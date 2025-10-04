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
        private bool isProcessComplete;

        public UnityEvent OnProcessComplete => onProcessComplete;

        public UnityEvent OnProcessFailed => onProcessFailed;

        public UnityEvent OnProcessStarted => onProcessStarted;

        public bool IsInProgress => isInProgress;

        public bool IsProcessComplete => isProcessComplete;

       
        public void Cancel()
        {
            var wasRunning = isInProgress;
            
            if (!wasRunning)
            {
                return;
            }
        
            isInProgress = false;
            isProcessComplete = false;
            CancelImpl();
            onProcessFailed.Invoke();
        }

        protected abstract void CancelImpl();
        

        public void Complete()
        {

            isInProgress = false;
            isProcessComplete = true;
            
            CompleteImpl();
            
            onProcessComplete.Invoke();
        }
        
        protected abstract void CompleteImpl();

     

      

        private void BeginProcess()
        {

            Cancel();

            isInProgress = true;
            isProcessComplete = false;
            BeginProcessImpl();
            onProcessStarted.Invoke();

        }
        
        protected abstract void BeginProcessImpl();

       
    }
}
