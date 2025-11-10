using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Executes a list of process actions either in parallel (all at once) or sequentially (one after another).
    /// Unlike SequenceAction, this is a simpler implementation without delays, time limits, or looping.
    /// </summary>
    [Serializable]
    [JungleClassInfo("List Process Action", "Runs multiple process actions in parallel or sequence.", null, "Actions/Process")]
    public class ListProcessAction : IProcessAction
    {
        /// <summary>
        /// Selects how the list executes its child processes.
        /// </summary>
        public enum ExecutionMode
        {
            Parallel,   // Execute all processes at once
            Sequential  // Execute one process after another
        }

        [SerializeField] 
        private ExecutionMode mode = ExecutionMode.Parallel;
        /// <summary>
        /// Defines the child process actions to execute.
        /// </summary>

        [SerializeReference] 
        [JungleClassSelection]
        public List<IProcessAction> Processes = new List<IProcessAction>();

        private bool isInProgress;
        private bool hasCompleted;
        private int currentProcessIndex;
        private readonly HashSet<IProcessAction> runningProcesses = new HashSet<IProcessAction>();
        private Action completionCallback;

        public event Action OnProcessCompleted;
        /// <summary>
        /// Indicates whether the action can report a finite duration.
        /// </summary>

        public bool HasDefinedDuration
        {
            get
            {
                if (Processes == null || Processes.Count == 0)
                    return true;

                bool allHaveDefinedDuration = true;
                foreach (var process in Processes)
                {
                    if (process == null || !process.HasDefinedDuration)
                    {
                        allHaveDefinedDuration = false;
                        break;
                    }
                }

                return allHaveDefinedDuration;
            }
        }
        /// <summary>
        /// Gets the total duration of the action in seconds.
        /// </summary>

        public float Duration
        {
            get
            {
                if (Processes == null || Processes.Count == 0)
                    return 0f;

                if (!HasDefinedDuration)
                    return -1f;

                float totalDuration = 0f;

                if (mode == ExecutionMode.Sequential)
                {
                    // Sequential: sum all durations
                    foreach (var process in Processes)
                    {
                        if (process != null)
                            totalDuration += process.Duration;
                    }
                }
                else
                {
                    // Parallel: take the longest duration
                    foreach (var process in Processes)
                    {
                        if (process != null && process.Duration > totalDuration)
                            totalDuration = process.Duration;
                    }
                }

                return totalDuration;
            }
        }
        /// <summary>
        /// Gets whether the action is currently running.
        /// </summary>

        public bool IsInProgress => isInProgress;
        /// <summary>
        /// Gets whether the action has finished executing.
        /// </summary>

        public bool HasCompleted => hasCompleted;

        /// <summary>
        /// Starts the list process action.
        /// </summary>
        /// <param name="callback"></param>
        public void Start(Action callback = null)
        {
            if (isInProgress)
                return;

            isInProgress = true;
            hasCompleted = false;
            currentProcessIndex = 0;
            runningProcesses.Clear();
            completionCallback = callback;

            if (Processes == null || Processes.Count == 0)
            {
                Complete();
                return;
            }

            if (mode == ExecutionMode.Parallel)
            {
                StartParallelExecution();
            }
            else
            {
                StartSequentialExecution();
            }
        }
        /// <summary>
        /// Stops the list process action before completion.
        /// </summary>

        public void Stop()
        {
            if (!isInProgress)
                return;

            isInProgress = false;
            hasCompleted = false;
            completionCallback = null;

            // Stop all running processes
            foreach (var process in runningProcesses)
            {
                if (process != null)
                {
                    process.Stop();
                }
            }

            runningProcesses.Clear();
        }

        private void StartParallelExecution()
        {
            // Start all processes at once
            foreach (var process in Processes)
            {
                if (process != null)
                {
                    StartChildProcess(process, HandleParallelCompletion);
                }
            }

            // If no valid processes were started, complete immediately
            if (runningProcesses.Count == 0)
            {
                Complete();
            }
        }

        private void StartSequentialExecution()
        {
            // Start the first process
            StartNextSequentialProcess();
        }

        private void StartNextSequentialProcess()
        {
            // Find the next valid process
            while (currentProcessIndex < Processes.Count)
            {
                var process = Processes[currentProcessIndex];
                if (process != null)
                {
                    StartChildProcess(process, HandleSequentialCompletion);
                    return;
                }
                currentProcessIndex++;
            }

            // No more processes to start
            if (runningProcesses.Count == 0)
            {
                Complete();
            }
        }

        private void StartChildProcess(IProcessAction process, Action<IProcessAction> onCompleted)
        {
            runningProcesses.Add(process);
            process.Start(() => onCompleted?.Invoke(process));
        }

        private void HandleParallelCompletion(IProcessAction process)
        {
            if (!isInProgress)
                return;

            runningProcesses.Remove(process);

            if (runningProcesses.Count == 0)
            {
                Complete();
            }
        }

        private void HandleSequentialCompletion(IProcessAction process)
        {
            if (!isInProgress)
                return;

            runningProcesses.Remove(process);
            currentProcessIndex++;
            StartNextSequentialProcess();
        }

        private void Complete()
        {
            if (!isInProgress)
                return;

            isInProgress = false;
            hasCompleted = true;

            // Clean up any remaining listeners
            foreach (var process in runningProcesses)
            {
                if (process != null)
                {
                }
            }

            runningProcesses.Clear();

            OnProcessCompleted?.Invoke();
            completionCallback?.Invoke();
            completionCallback = null;
        }
    }
}
