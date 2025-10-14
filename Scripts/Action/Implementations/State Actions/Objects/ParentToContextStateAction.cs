using System.Collections.Generic;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    [JungleClassInfo(
        "Moves a GameObject under a drag contextual parent transform during drag operations.",
        "d_UnityEditor.HierarchyWindow")]
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