using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    
    public enum ColliderStateModification
    {
        Enable,
        Disable,
        Original,
        Toggle
    }
    
    [System.Serializable]
    public class ColliderStateStateAction : IStateAction
    {
        [SerializeReference][JungleClassSelection] private IColliderValue targetColliders;
        [SerializeField] private ColliderStateModification onExecuteModification;
        [SerializeField] private ColliderStateModification onCompleteModification;

        private readonly Dictionary<Collider, bool> originalStates = new();
      
       
        public void OnStateEnter()
        {
            StoreOriginalState();
            ApplyColliderStates(onExecuteModification);
        }

        private void StoreOriginalState()
        {
            originalStates.Clear();
            foreach (var collider in targetColliders.Values)
            {
                originalStates[collider] = collider.enabled;
            }
        }

        public void OnStateExit()
        {

            ApplyColliderStates(onCompleteModification);
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
