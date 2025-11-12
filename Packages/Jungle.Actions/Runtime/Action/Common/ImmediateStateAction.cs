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
    /// Executes an immediate action before and/or after the configured invocation.
    /// </summary>


    [JungleClassInfo("Immediate State Action", "Executes an immediate action before and/or after the configured invocation.", null, "Actions/Logic")]
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
        public void StartProcess(Action callback = null)
        {
            if (executionMode != ImmediateActionExecute.OnEnd)
            {
                action?.StartProcess();
            }

            callback?.Invoke();

            if (executionMode != ImmediateActionExecute.OnBegin)
            {
                action?.StartProcess();
            }
        }
    }
}
