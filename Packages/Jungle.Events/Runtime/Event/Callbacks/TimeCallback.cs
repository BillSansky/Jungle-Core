using System;
using System.Collections;
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

        private Action callbackAction;
        private Coroutine routine;

        /// <inheritdoc />
        public void StartMonitoring(Action callbackAction)
        {

            EndMonitoring();
            this.callbackAction = callbackAction;
            routine = CoroutineRunner.StartManagedCoroutine(WaitRoutine());
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

            NotifyCallbackAction();
            routine = null;
        }

        private void NotifyCallbackAction()
        {
            callbackAction.Invoke();
        }
    }
}
