using System;
using Jungle.Actions;
using Jungle.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    /// <summary>
    /// Invokes UnityEvents when actions start, stop, or on one-shot.
    /// </summary>
    [JungleClassInfo("Unity Event Action", "Invokes UnityEvents when actions start, stop, or on one-shot.", "d_UnityEvent Icon", "Actions/Events")]
    [Serializable]
    public class UnityEventAction : IImmediateAction
    {
        /// <summary>
        /// Invoked when the state becomes active.
        /// </summary>
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onStop = new();
        [SerializeField] private UnityEvent onOneShot = new();

        private bool hasExecuted;

        public void StartProcess(Action callback = null)
        {
            if (hasExecuted)
            {
                return;
            }

            onStart?.Invoke();

            hasExecuted = true;
            callback?.Invoke();
        }

        public void Stop()
        {
            if (!hasExecuted)
            {
                return;
            }

            onStop?.Invoke();

            hasExecuted = false;
        }


    }
}
