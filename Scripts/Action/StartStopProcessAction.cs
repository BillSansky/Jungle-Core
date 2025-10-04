using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    public class StartStopProcessAction : ProcessAction
    {
        [JungleClassSelection]
        [SerializeReference]
        private StartStopAction startStopAction;

        public override bool IsTimed => false;

        protected override void BeginProcessImpl()
        {
            GetAction().Start();
        }

        protected override void CancelImpl()
        {
            GetAction().Stop();
        }

        protected override void CompleteImpl()
        {
            GetAction().Stop();
        }

        private StartStopAction GetAction()
        {
            if (startStopAction == null)
            {
                throw new InvalidOperationException("StartStopProcessAction requires a StartStopAction reference.");
            }

            return startStopAction;
        }
    }
}
