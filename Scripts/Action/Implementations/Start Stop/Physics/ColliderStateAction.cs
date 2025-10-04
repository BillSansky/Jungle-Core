using System.Collections.Generic;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    public class ColliderStateAction : StartStopAction
    {
        [SerializeReference] private List<IColliderValue> targetColliders = new();
        [SerializeField] private ColliderEnabledState startAction;
        [SerializeField] private ColliderEnabledState stopAction;

        private readonly Dictionary<Collider, bool> originalStates = new();
        private bool skipStop;
        

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
            StoreOriginalState();
            ApplyColliderStates(startAction);
        }

        private void StoreOriginalState()
        {
            originalStates.Clear();
            foreach (var collider in targetColliders)
            {
                originalStates[collider.Value()] = collider.Value().enabled;
            }
        }

        protected override void OnStop()
        {
            if (skipStop)
            {
                return;
            }

            ApplyColliderStates(stopAction);
        }


        private void ApplyColliderStates(ColliderEnabledState action)
        {
            foreach (var collider in targetColliders)
            {
                if (!collider.Value()) continue;

                switch (action)
                {
                    case ColliderEnabledState.Enable:
                        collider.Value().enabled = true;
                        break;
                    case ColliderEnabledState.Disable:
                        collider.Value().enabled = false;
                        break;
                    case ColliderEnabledState.Original:
                        if (originalStates.TryGetValue(collider.Value(), out bool originalState))
                            collider.Value().enabled = originalState;
                        break;
                    case ColliderEnabledState.Toggle:
                        collider.Value().enabled = !collider.Value().enabled;
                        break;
                }
            }
        }
        
    }
}
