using System.Collections;
using System.Collections.Generic;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    public class DelayedActionGroupAction : BeginCompleteAction
    {
        [SerializeField]
        private float startDelay = 1.0f;

        [SerializeReference] private List<ProcessAction> actionsToExecute = new();
        [SerializeField] private bool cancelStartOnStop = true;

        [SerializeField]
        private bool useStopDelay;

        [SerializeField] private float stopDelay = 1.0f;
        [SerializeField] private bool cancelStopOnStart = true;

      

        private Coroutine startDelayCoroutine;
        private Coroutine stopDelayCoroutine;
        private bool isWaitingForStartDelay;
        private bool isWaitingForStopDelay;
        private bool actionsCurrentlyRunning;

        public override bool IsTimed => true;
        public override float Duration => startDelay + (useStopDelay ? stopDelay : 0f);
      
        public void StartAction()
        {
            Start();
        }

        public void StopAction()
        {
            Stop();
        }

      

        protected override void OnStart()
        {
            // Cancel any pending stop operation if configured to do so
            if (cancelStopOnStart && stopDelayCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(stopDelayCoroutine);
                stopDelayCoroutine = null;
                isWaitingForStopDelay = false;
            }

            // Cancel any existing start delay
            if (startDelayCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(startDelayCoroutine);
            }

            startDelayCoroutine = CoroutineRunner.StartManagedCoroutine(StartDelayedExecutionCoroutine());
        }

        protected override void OnStop()
        {
            // Cancel any pending start operation if configured to do so
            if (cancelStartOnStop && startDelayCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(startDelayCoroutine);
                startDelayCoroutine = null;
                isWaitingForStartDelay = false;
            }

            if (useStopDelay && actionsCurrentlyRunning)
            {
                // Cancel any existing stop delay
                if (stopDelayCoroutine != null)
                {
                    CoroutineRunner.StopManagedCoroutine(stopDelayCoroutine);
                }

                stopDelayCoroutine = CoroutineRunner.StartManagedCoroutine(StopDelayedExecutionCoroutine());
            }
            else
            {
                // Stop immediately without delay
                StopActionsImmediately();
            }
        }
        

        private IEnumerator CompleteOneShotCoroutine()
        {
            // Start phase: wait for start delay and execute
            isWaitingForStartDelay = true;

            yield return new WaitForSeconds(startDelay);

            isWaitingForStartDelay = false;

            // Execute the actions
            ExecuteActions();

            // Let the actions run for a moment
            yield return new WaitForSeconds(0.1f);

            // Stop phase: apply stop delay if configured
            if (useStopDelay)
            {
                isWaitingForStopDelay = true;

                yield return new WaitForSeconds(stopDelay);

                isWaitingForStopDelay = false;
            }

            // Stop all actions
            StopActionsImmediately();
        }

        private IEnumerator StartDelayedExecutionCoroutine()
        {
            if (isWaitingForStartDelay)
            {
                // Prevent re-entry if already waiting
                yield break;
            }

            isWaitingForStartDelay = true;

            yield return new WaitForSeconds(startDelay);

            isWaitingForStartDelay = false;

            ExecuteActions();
        }

        private IEnumerator StopDelayedExecutionCoroutine()
        {
            if (isWaitingForStopDelay)
            {
                yield break;
            }

            isWaitingForStopDelay = true;

            yield return new WaitForSeconds(stopDelay);

            isWaitingForStopDelay = false;

            StopActionsImmediately();
        }

        private void ExecuteActions()
        {
            if (actionsCurrentlyRunning)
            {
                return;
            }

            actionsCurrentlyRunning = true;

            foreach (var action in actionsToExecute)
            {
               action.Start();
            }
        }

        private void StopActionsImmediately()
        {
            if (!actionsCurrentlyRunning) return;

            foreach (var action in actionsToExecute)
            {
               action.Stop();
            }

            actionsCurrentlyRunning = false;
        }

        // Public properties for runtime access
        public float StartDelay
        {
            get => startDelay;
            set => startDelay = Mathf.Max(0f, value);
        }

        public float StopDelay
        {
            get => stopDelay;
            set => stopDelay = Mathf.Max(0f, value);
        }

        public bool CancelStartOnStop
        {
            get => cancelStartOnStop;
            set => cancelStartOnStop = value;
        }

        public bool CancelStopOnStart
        {
            get => cancelStopOnStart;
            set => cancelStopOnStart = value;
        }

        public bool UseStopDelay
        {
            get => useStopDelay;
            set => useStopDelay = value;
        }

        public bool IsWaitingForStartDelay => isWaitingForStartDelay;
        public bool IsWaitingForStopDelay => isWaitingForStopDelay;
        public bool ActionsCurrentlyRunning => actionsCurrentlyRunning;

        // Methods to modify the action list at runtime
        public void AddAction(ProcessAction action)
        {
            if (action != null && !actionsToExecute.Contains(action))
            {
                actionsToExecute.Add(action);
            }
        }

        public void RemoveAction(ProcessAction action)
        {
            actionsToExecute.Remove(action);
        }

        public void ClearActions()
        {
            actionsToExecute.Clear();
        }

        // Force immediate execution/stopping (bypassing delays)
        public void ForceExecuteImmediately()
        {
            // Cancel any pending delays
            if (startDelayCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(startDelayCoroutine);
                startDelayCoroutine = null;
                isWaitingForStartDelay = false;
            }

            ExecuteActions();
        }

        public void ForceStopImmediately()
        {
            // Cancel any pending delays
            if (stopDelayCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(stopDelayCoroutine);
                stopDelayCoroutine = null;
                isWaitingForStopDelay = false;
            }

            StopActionsImmediately();
        }

        private void OnDisable()
        {
            // Clean up coroutines when disabled
            if (startDelayCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(startDelayCoroutine);
                startDelayCoroutine = null;
            }

            if (stopDelayCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(stopDelayCoroutine);
                stopDelayCoroutine = null;
            }

            isWaitingForStartDelay = false;
            isWaitingForStopDelay = false;
        }
    }
}
