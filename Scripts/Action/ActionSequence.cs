using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    public class ActionSequence : ProcessAction
    {

        [JungleClassSelection] [SerializeReference]
        public List<Step> Actions;

        [Serializable]
        public class Step
        {
            public ProcessAction Action;
            public bool loopTillEnd;
            //whether the next actions will only start when that one is done
            public bool isLinear;
        }


        protected override void CancelImpl()
        {
            throw new NotImplementedException();
        }

        protected override void CompleteImpl()
        {
            throw new NotImplementedException();
        }

        protected override void BeginProcessImpl()
        {
            throw new NotImplementedException();
        }
    }
}