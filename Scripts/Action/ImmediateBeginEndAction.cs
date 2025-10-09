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
    
    
    [Serializable]
    public class ImmediateBeginEndAction : IBeginEndAction
    {
        [SerializeReference][JungleClassSelection]
        private IImmediateAction action;
        
        public ImmediateActionExecute executionMode = ImmediateActionExecute.OnBeginAndEnd;
        
        public virtual void Begin()
        {
            action.Execute();
        }
        
        public virtual void End()
        {
            action.Execute();
        }
    }
}