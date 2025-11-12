using System;
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
    /// Enables, disables, or toggles colliders when the action executes.
    /// </summary>
    
    [Serializable]
    [JungleClassInfo("Collider State Action", "Enables, disables, or toggles colliders when the action executes.", null, "Actions/Physics")]
    public class ColliderStateStateAction : IImmediateAction
    {
        [SerializeReference][JungleClassSelection] private IColliderValue targetColliders;
        [SerializeField] private ColliderStateModification onExecuteModification;

        public void StartProcess(Action callback = null)
        {
            ApplyColliderStates(onExecuteModification);
            callback?.Invoke();
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
                        break;
                    case ColliderStateModification.Toggle:
                        collider.enabled = !collider.enabled;
                        break;
                }
            }
        }
        
    }
    
    
  
}
