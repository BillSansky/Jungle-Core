using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Enumerates the ColliderStateModification values.
    /// </summary>
    public enum ColliderStateModification
    {
        Enable,
        Disable,
        Original,
        Toggle
    }
    /// <summary>
    /// Enables or disables Collider components when the owning state enters or exits.
    /// </summary>
    [System.Serializable]
    public class ColliderStateStateAction : IStateAction
    {
        [SerializeReference][JungleClassSelection] private IColliderValue targetColliders;
        [SerializeField] private ColliderStateModification onExecuteModification;
        [SerializeField] private ColliderStateModification onCompleteModification;

        private readonly Dictionary<Collider, bool> originalStates = new();
        /// <summary>
        /// Handles the OnStateEnter event.
        /// </summary>
        public void OnStateEnter()
        {
            StoreOriginalState();
            ApplyColliderStates(onExecuteModification);
        }
        /// <summary>
        /// Captures the initial enabled state for each collider so it can be restored later.
        /// </summary>
        private void StoreOriginalState()
        {
            originalStates.Clear();
            foreach (var collider in targetColliders.Values)
            {
                originalStates[collider] = collider.enabled;
            }
        }
        /// <summary>
        /// Handles the OnStateExit event.
        /// </summary>
        public void OnStateExit()
        {

            ApplyColliderStates(onCompleteModification);
        }
        /// <summary>
        /// Applies the requested modification to every targeted collider.
        /// </summary>
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
