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
    }
    
    [Serializable]
    public class TimedStateActionExecutor : IProcessAction
    {
        
        [SerializeReference][JungleClassSelection]
        public List<IStateAction> Actions  = new List<IStateAction>();
        
        public EndLogic endLogic = EndLogic.Timed;
        
        public float duration = 1.0f;

        [NonSerialized]
        public MonoBehaviour coroutineRunner;

        private Coroutine autoEndCoroutine;
        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => true;

        public float Duration
        {
            get
            {
                switch (endLogic)
                {
                    case EndLogic.Timed:
                        return duration;
                    case EndLogic.OneFrame:
                        return 0.0f;
                    default:
                        return 0.0f;
                }
            }
        }

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;

        public void Execute()
        {
            Start();
        }

        public void Start()
        {
            Debug.Assert(coroutineRunner != null, "coroutineRunner must be set before starting the action");

            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;

            foreach (var action in Actions)
            {
                action?.OnStateEnter();
            }

            // Start auto-end coroutine if needed
            if ((endLogic == EndLogic.Timed || endLogic == EndLogic.OneFrame) && coroutineRunner != null)
            {
                autoEndCoroutine = coroutineRunner.StartCoroutine(AutoEndCoroutine());
            }
        }

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            Cleanup();
            isInProgress = false;
            hasCompleted = false;
        }

        private void Cleanup()
        {
            // Stop the auto-end coroutine if it's running
            if (autoEndCoroutine != null && coroutineRunner != null)
            {
                coroutineRunner.StopCoroutine(autoEndCoroutine);
                autoEndCoroutine = null;
            }

            foreach (var action in Actions)
            {
                action?.OnStateExit();
            }
        }

        private void Complete()
        {
            if (!isInProgress)
            {
                return;
            }

            Cleanup();
            isInProgress = false;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
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

            Complete();
        }
    }
}