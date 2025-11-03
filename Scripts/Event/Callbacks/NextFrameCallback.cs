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
    public sealed class NextFrameCallback : ICallback
    {
        private readonly List<Action> callbackActions = new();

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
        }

        /// <inheritdoc />
        public void Invoke()
        {
            CoroutineRunner.StartManagedCoroutine(WaitForFrame());
        }

        private IEnumerator WaitForFrame()
        {
            yield return null;
            NotifyCallbackActions();
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
