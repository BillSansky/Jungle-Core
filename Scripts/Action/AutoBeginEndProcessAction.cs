using System;
using System.Collections;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    public enum EndLogic
    {
        Timed,
        OneFrame,
        NeverEnd,
    }
    
    [Serializable]
    public class AutoBeginEndProcessAction : ProcessAction
    {
        
        [SerializeReference][JungleClassSelection]
        public List<IBeginEndAction> Actions  = new List<IBeginEndAction>();
        
        public EndLogic endLogic = EndLogic.Timed;
        
        public float duration = 1.0f;

        [NonSerialized]
        public MonoBehaviour coroutineRunner;

        private Coroutine autoEndCoroutine;

        private event Action internalCompletionListener;
        
        public override bool IsTimed => endLogic != EndLogic.NeverEnd;

        public override float Duration
        {
            get
            {
                switch (endLogic)
                {
                    case EndLogic.Timed:
                        return duration;
                    case EndLogic.OneFrame:
                        return 0.0f;
                    case EndLogic.NeverEnd:
                        return float.PositiveInfinity;
                    default:
                        return 0.0f;
                }
            }
        }

        protected override void BeginImpl()
        {
            foreach (var action in Actions)
            {
                action?.Begin();
            }

            // Start auto-end coroutine if needed
            if ((endLogic == EndLogic.Timed || endLogic == EndLogic.OneFrame) && coroutineRunner != null)
            {
                autoEndCoroutine = coroutineRunner.StartCoroutine(AutoEndCoroutine());
            }
        }

        protected override void InterruptOrCompleteCleanup()
        {
            // Stop the auto-end coroutine if it's running
            if (autoEndCoroutine != null && coroutineRunner != null)
            {
                coroutineRunner.StopCoroutine(autoEndCoroutine);
                autoEndCoroutine = null;
            }

            foreach (var action in Actions)
            {
                action?.End();
            }
        }

        protected override void RegisterInternalCompletionListener(Action onCompleted)
        {
            internalCompletionListener+=onCompleted;
        }

        private IEnumerator AutoEndCoroutine()
        {
            if (endLogic == EndLogic.OneFrame)
            {
                yield return null;
            }
            else if (endLogic == EndLogic.Timed)
            {
                yield return new WaitForSeconds(duration);
            }

            internalCompletionListener?.Invoke();
        }
    }
}