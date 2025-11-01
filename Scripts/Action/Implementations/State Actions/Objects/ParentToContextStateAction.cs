using System;
using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    [JungleClassInfo(
        "Parent To Context Action",
        "Moves a GameObject under a contextual parent transform during the active state.",
        "d_UnityEditor.HierarchyWindow",
        "Actions/State")]
    /// <summary>
    /// Implements the parent to context state action action.
    /// </summary>
    public class ParentToContextStateAction : IStateAction
    {
        [Header("Target Configuration")] [SerializeField]
        private ITransformValue targetTransforms;

        [Header("Parent Configuration")] [SerializeReference] [JungleClassSelection]
        private ITransformValue parentTransform;

        [Header("Movement Options")] [SerializeField]
        private bool preserveWorldPosition = true;


        // Cache original parents for potential reversion
        private List<Transform> originalParents = new();
        private bool skipStop;
        /// <summary>
        /// Invoked when the state becomes active.
        /// </summary>

        public void OnStateEnter()
        {
            originalParents.Clear();
            foreach (var target in targetTransforms.Values)
            {
                if (target != null)
                {
                    originalParents.Add(target.parent);
                }
            }


            // Set new parent
            var parent = parentTransform.V;

            foreach (var t in targetTransforms.Values)
            {
                t.SetParent(parent, preserveWorldPosition);
            }
        }
        /// <summary>
        /// Invoked when the state finishes running.
        /// </summary>

        public void OnStateExit()
        {
            if (skipStop)
            {
                return;
            }

            int i = 0;
            // Revert objects to their original parents
            foreach (var trans in targetTransforms.Values)
            {
                trans.SetParent(originalParents[i], preserveWorldPosition);
                i++;
            }

            // Clear cached references
            originalParents.Clear();
        }
    }
}