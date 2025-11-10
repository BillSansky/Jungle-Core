using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Implements the collider state modification action.
    /// </summary>
    
    public enum ColliderStateModification
    {
        Enable,
        Disable,
        Original,
        Toggle
    }
    /// <summary>
    /// Enables, disables, or toggles colliders during the state lifecycle.
    /// </summary>
    
    [Serializable]
    [JungleClassInfo("Collider State Action", "Enables, disables, or toggles colliders during the state lifecycle.", null, "Actions/State")]
    public class ColliderStateStateAction : IImmediateAction
    {
        [SerializeReference][JungleClassSelection] private IColliderValue targetColliders;
        [SerializeField] private ColliderStateModification onExecuteModification;
        [SerializeField] private ColliderStateModification onCompleteModification;

        private readonly Dictionary<Collider, bool> originalStates = new();

        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => true;

        public float Duration => 0f;

        public bool IsInProgress => isInProgress;

        public bool HasCompleted => hasCompleted;

        public void Start(Action callback = null)
        {
            if (isInProgress)
            {
                return;
            }

            StoreOriginalState();
            ApplyColliderStates(onExecuteModification);

            isInProgress = true;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
            callback?.Invoke();
        }

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            ApplyColliderStates(onCompleteModification);

            isInProgress = false;
            hasCompleted = false;
        }

        private void StoreOriginalState()
        {
            originalStates.Clear();
            foreach (var collider in targetColliders.Values)
            {
                originalStates[collider] = collider.enabled;
            }
        }


        private void ApplyColliderStates(ColliderStateModification action)
        {
            foreach (var collider in targetColliders.Values)
            {
                if (!collider) continue;

                switch (action)
                {
                    case ColliderStateModification.Enable:
                        collider.enabled = true;
                        break;
                    case ColliderStateModification.Disable:
                        collider.enabled = false;
                        break;
                    case ColliderStateModification.Original:
                        if (originalStates.TryGetValue(collider, out bool originalState))
                            collider.enabled = originalState;
                        break;
                    case ColliderStateModification.Toggle:
                        collider.enabled = !collider.enabled;
                        break;
                }
            }
        }
        
    }
    
    
  
}
