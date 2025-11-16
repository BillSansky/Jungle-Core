using System;
using System.Collections;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Triggers callback actions after waiting for a single rendered frame.
    /// </summary>
    [Serializable]
    public sealed class NextFrameCallback : IEventMonitor
    {
        private Action callbackAction;
        private Coroutine routine;

        /// <inheritdoc />
        public void StartMonitoring(Action callbackAction)
        {
            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

            EndMonitoring();
            this.callbackAction = callbackAction;
            routine = CoroutineRunner.StartManagedCoroutine(WaitForFrame());
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            if (routine == null)
            {
                callbackAction = null;
                return;
            }

            CoroutineRunner.StopManagedCoroutine(routine);
            routine = null;
            callbackAction = null;
        }

        private IEnumerator WaitForFrame()
        {
            yield return null;
            NotifyCallbackAction();
            routine = null;
        }

        private void NotifyCallbackAction()
        {
            var action = callbackAction;
            callbackAction = null;
            action?.Invoke();
        }
    }
}
