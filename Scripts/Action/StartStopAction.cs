using System;

namespace Jungle.Actions
{
    /// <summary>
    /// Base class for actions that support starting and stopping their execution.
    /// </summary>
    [Serializable]
    public abstract class StartStopAction
    {
        private bool isStarted;

        
        protected bool IsStarted => isStarted;


        public void Start()
        {
            if (isStarted)
            {
                return;
            }

            isStarted = true;
            OnStart();
        }

        /// <summary>
        /// Stops the action if it has been started previously.
        /// </summary>
        public void Stop()
        {
            if (!isStarted)
            {
                return;
            }

            isStarted = false;
            OnStop();
        }
        

        /// <summary>
        /// Called when the action starts.
        /// </summary>
        protected abstract void OnStart();

        /// <summary>
        /// Called when the action stops.
        /// </summary>
        protected abstract void OnStop();
    }
}
