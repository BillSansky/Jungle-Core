using System;

namespace Jungle.Actions
{
    /// <summary>
    /// Base class for actions that support starting and stopping their execution.
    /// </summary>
    [Serializable]
    public abstract class StartStopAction : Action
    {
        private bool isStarted;

        /// <summary>
        /// Indicates whether the action has been started.
        /// </summary>
        protected bool IsStarted => isStarted;

        /// <summary>
        /// Starts the action. This method is also invoked when the action is executed.
        /// </summary>
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
        /// Executes the action. For start/stop actions execution simply starts them.
        /// </summary>
        public sealed override void Execute()
        {
            Start();
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
