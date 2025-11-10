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
    [JungleClassInfo("Unity Event Action", "Invokes UnityEvents when actions start, stop, or on one-shot.", "d_UnityEvent Icon", "Actions/State")]
    [Serializable]
    public class UnityEventAction : IImmediateAction
    {
        /// <summary>
        /// Invoked when the state becomes active.
        /// </summary>
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onStop = new();
        [SerializeField] private UnityEvent onOneShot = new();

        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => true;

        public float Duration => 0f;

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;

        public void Start(Action callback = null)
        {
            if (isInProgress)
            {
                return;
            }

            onStart?.Invoke();

            isInProgress = true;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            callback?.Invoke();
        }

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            onStop?.Invoke();

            isInProgress = false;
            hasCompleted = false;
        }


    }
}
