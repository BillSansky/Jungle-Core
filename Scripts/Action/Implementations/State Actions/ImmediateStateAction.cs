using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    public enum ImmediateActionExecute
    {
        OnBegin,
        OnEnd,
        OnBeginAndEnd
    }


    [JungleClassInfo("Immediate Action", "Executes an immediate action when the state is entered or exited.")]
    [Serializable]
    public class ImmediateStateAction : IStateAction
    {
        [SerializeReference] [JungleClassSelection]
        private IImmediateAction action;

        public ImmediateActionExecute executionMode = ImmediateActionExecute.OnBeginAndEnd;

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