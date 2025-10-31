using Jungle.Conditions;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Checks whether a process runner currently has any active processes.
    /// </summary>
    [System.Serializable]
    public class ProcessRunnerCondition : Condition
    {
        /// <summary>
        /// Enumerates the ProcessState values.
        /// </summary>
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
        /// <summary>
        /// Matches the linked process runner's state against the expected status value.
        /// </summary>
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
