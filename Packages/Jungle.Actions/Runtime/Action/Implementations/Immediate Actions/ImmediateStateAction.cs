using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Specifies when the immediate action should execute.
    /// </summary>
    public enum ImmediateActionExecute
    {
        OnBegin,
        OnEnd,
        OnBeginAndEnd
    }
    /// <summary>
    /// Executes an immediate action when the state is entered or exited.
    /// </summary>


    [JungleClassInfo("Immediate State Action", "Executes an immediate action when the state is entered or exited.", null, "Actions/State")]
    [Serializable]
    public class ImmediateStateAction : IImmediateAction
    {
        [SerializeReference]
        [JungleClassSelection(typeof(IImmediateAction))]
        private IImmediateAction action;
        /// <summary>
        /// Controls whether the action runs on enter, exit, or both.
        /// </summary>

        public ImmediateActionExecute executionMode = ImmediateActionExecute.OnBeginAndEnd;
        /// <summary>
        /// Invoked when the state becomes active.
        /// </summary>

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

            if (executionMode is ImmediateActionExecute.OnBegin or ImmediateActionExecute.OnBeginAndEnd)
            {
                action?.Start();
            }

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

            if (executionMode is ImmediateActionExecute.OnEnd or ImmediateActionExecute.OnBeginAndEnd)
            {
                action?.Start();
            }

            isInProgress = false;
            hasCompleted = false;
        }
    }
}
