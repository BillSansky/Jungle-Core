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

        private bool hasStarted;

        public void StartProcess(Action callback = null)
        {
            if (executionMode is ImmediateActionExecute.OnBegin or ImmediateActionExecute.OnBeginAndEnd)
            {
                action?.StartProcess();
            }

            hasStarted = true;
            callback?.Invoke();
        }

        public void Stop()
        {
            if (!hasStarted)
            {
                return;
            }

            if (executionMode is ImmediateActionExecute.OnEnd or ImmediateActionExecute.OnBeginAndEnd)
            {
                action?.StartProcess();
            }

            hasStarted = false;
        }
    }
}
