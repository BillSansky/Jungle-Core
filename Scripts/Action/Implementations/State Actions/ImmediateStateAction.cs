using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Enumerates the ImmediateActionExecute values.
    /// </summary>
    public enum ImmediateActionExecute
    {
        OnBegin,
        OnEnd,
        OnBeginAndEnd
    }
    /// <summary>
    /// Runs a nested immediate action as soon as the state becomes active.
    /// </summary>
    [JungleClassInfo("Immediate Action", "Executes an immediate action when the state is entered or exited.")]
    [Serializable]
    public class ImmediateStateAction : IStateAction
    {
        [SerializeReference] [JungleClassSelection]
        private IImmediateAction action;

        public ImmediateActionExecute executionMode = ImmediateActionExecute.OnBeginAndEnd;
        /// <summary>
        /// Handles the OnStateEnter event.
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
        /// Handles the OnStateExit event.
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