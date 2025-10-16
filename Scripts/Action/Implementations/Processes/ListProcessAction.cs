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
    public class ListProcessAction : IProcessAction
    {
        public enum ExecutionMode
        {
            Parallel,   // Execute all processes at once
            Sequential  // Execute one process after another
        }

        [SerializeField] 
        private ExecutionMode mode = ExecutionMode.Parallel;

        [SerializeReference] 
        [JungleClassSelection]
        public List<IProcessAction> Processes = new List<IProcessAction>();

        public event Action OnProcessCompleted;

        private bool isInProgress;
        private bool hasCompleted;
        private int currentProcessIndex;
        private readonly HashSet<IProcessAction> runningProcesses = new HashSet<IProcessAction>();

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

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;

        public void Start()
        {
            if (isInProgress)
                return;

            isInProgress = true;
            hasCompleted = false;
            currentProcessIndex = 0;
            runningProcesses.Clear();

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

        public void Interrupt()
        {
            if (!isInProgress)
                return;

            isInProgress = false;

            // Interrupt all running processes
            foreach (var process in runningProcesses)
            {
                if (process != null)
                {
                    process.OnProcessCompleted -= OnChildProcessCompleted;
                    process.Interrupt();
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
                    runningProcesses.Add(process);
                    process.OnProcessCompleted += OnChildProcessCompleted;
                    process.Start();
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
                    runningProcesses.Add(process);
                    process.OnProcessCompleted += OnChildProcessCompleted;
                    process.Start();
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

        private void OnChildProcessCompleted()
        {
            if (!isInProgress)
                return;

            if (mode == ExecutionMode.Parallel)
            {
                HandleParallelCompletion();
            }
            else
            {
                HandleSequentialCompletion();
            }
        }

        private void HandleParallelCompletion()
        {
            // Find which process completed
            IProcessAction completedProcess = null;
            foreach (var process in runningProcesses)
            {
                if (process != null && process.HasCompleted)
                {
                    completedProcess = process;
                    break;
                }
            }

            if (completedProcess != null)
            {
                completedProcess.OnProcessCompleted -= OnChildProcessCompleted;
                runningProcesses.Remove(completedProcess);
            }

            // Check if all processes are complete
            if (runningProcesses.Count == 0)
            {
                Complete();
            }
        }

        private void HandleSequentialCompletion()
        {
            // Remove completed process
            IProcessAction completedProcess = null;
            foreach (var process in runningProcesses)
            {
                if (process != null && process.HasCompleted)
                {
                    completedProcess = process;
                    break;
                }
            }

            if (completedProcess != null)
            {
                completedProcess.OnProcessCompleted -= OnChildProcessCompleted;
                runningProcesses.Remove(completedProcess);
            }

            // Move to next process
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
                    process.OnProcessCompleted -= OnChildProcessCompleted;
                }
            }

            runningProcesses.Clear();

            OnProcessCompleted?.Invoke();
        }
    }
}
