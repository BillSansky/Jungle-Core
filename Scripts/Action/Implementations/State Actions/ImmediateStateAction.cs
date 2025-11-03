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
    public class ImmediateStateAction : IStateAction
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

        public virtual void OnStateEnter()
        {
            switch (executionMode)
            {
                case ImmediateActionExecute.OnBegin:
                case ImmediateActionExecute.OnBeginAndEnd:
                    action.Execute();
                    break;
                case ImmediateActionExecute.OnEnd:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        /// <summary>
        /// Invoked when the state finishes running.
        /// </summary>

        public virtual void OnStateExit()
        {
            switch (executionMode)
            {
                case ImmediateActionExecute.OnEnd:
                case ImmediateActionExecute.OnBeginAndEnd:
                    action.Execute();
                    break;
                case ImmediateActionExecute.OnBegin:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
