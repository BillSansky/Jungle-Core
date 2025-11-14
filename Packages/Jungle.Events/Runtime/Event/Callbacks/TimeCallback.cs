using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Events
{
    /// <summary>
    /// Executes registered callback actions after a configurable delay.
    /// </summary>
    [Serializable]
    public sealed class TimeCallback : IEventMonitor
    {
        [SerializeReference]
        [JungleClassSelection(typeof(IFloatValue))]
        private IFloatValue delay = new FloatValue(1f);

        [SerializeField]
        private bool useUnscaledTime;

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
            routine = CoroutineRunner.StartManagedCoroutine(WaitRoutine());
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

        private IEnumerator WaitRoutine()
        {
            var waitDuration = Mathf.Max(0f, delay?.Value() ?? 0f);

            if (useUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(waitDuration);
            }
            else
            {
                yield return new WaitForSeconds(waitDuration);
            }

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
