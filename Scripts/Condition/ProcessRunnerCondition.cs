﻿using Jungle.Conditions;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    public class ProcessRunnerCondition : Condition
    {
        public enum ProcessState
        {
            NotStarted,
            Running,
            Complete
        }

        [SerializeField]
        private ProcessRunner processRunner;

        [SerializeField]
        private ProcessState stateToCheck = ProcessState.Complete;

        protected internal override bool IsValidImpl()
        {
            if (processRunner == null)
            {
                return false;
            }

            switch (stateToCheck)
            {
                case ProcessState.NotStarted:
                    return !processRunner.IsRunning && !processRunner.IsComplete;
                case ProcessState.Running:
                    return processRunner.IsRunning;
                case ProcessState.Complete:
                    return processRunner.IsComplete;
                default:
                    return false;
            }
        }
    }
}
