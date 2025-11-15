using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly List<Action> callbackActions = new();
        private Coroutine routine;

        /// <inheritdoc />
        public void Attach(Action callbackAction)
        {
            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

            callbackActions.Add(callbackAction);
        }

        /// <inheritdoc />
        public void Detach(Action callbackAction)
        {
            if (callbackAction == null)
            {
                throw new ArgumentNullException(nameof(callbackAction));
            }

            callbackActions.Remove(callbackAction);

            if (callbackActions.Count == 0)
            {
                EndMonitoring();
            }
        }

        /// <inheritdoc />
        public void StartMonitoring()
        {
            if (callbackActions.Count == 0)
            {
                return;
            }

            EndMonitoring();
            routine = CoroutineRunner.StartManagedCoroutine(WaitForFrame());
        }

        /// <inheritdoc />
        public void EndMonitoring()
        {
            if (routine == null)
            {
                return;
            }

            CoroutineRunner.StopManagedCoroutine(routine);
            routine = null;
        }

        private IEnumerator WaitForFrame()
        {
            yield return null;
            NotifyCallbackActions();
            routine = null;
        }

        private void NotifyCallbackActions()
        {
            for (var index = 0; index < callbackActions.Count; index++)
            {
                callbackActions[index].Invoke();
            }
        }
    }
}
