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
    public class ParentToContextAction : ProcessAction
    {
        [Header("Target Configuration")] [SerializeField]
        private List<ITransformValue> targetTransforms;

        [Header("Parent Configuration")] [SerializeReference][JungleClassSelection]
        private ITransformValue parentTransform;

        [Header("Movement Options")] [SerializeField]
        private bool preserveWorldPosition = true;

        [SerializeField] private bool revertOnStop;

        // Cache original parents for potential reversion
        private List<Transform> originalParents = new ();
        private bool skipStop;
        
        public override bool IsTimed => false;
        public override float Duration => 0f;

      

        protected override void BeginImpl()
        {
            // Cache original parents if we need to revert
            if (revertOnStop)
            {
                originalParents.Clear();
                foreach (var target in targetTransforms)
                {
                    if (target != null)
                    {
                        originalParents.Add(target.V.parent);
                    }
                }
            }

            // Set new parent
            var parent = parentTransform.V;

            foreach (var t in targetTransforms)
            {
                t.V.SetParent(parent, preserveWorldPosition);
            }
        }

        protected override void CompleteImpl()
        {
            if (skipStop)
            {
                return;
            }

            if (!revertOnStop) return;

            // Revert objects to their original parents
            for (int i = 0; i < targetTransforms.Count && i < originalParents.Count; i++)
            {
                targetTransforms[i].V.SetParent(originalParents[i], preserveWorldPosition);
            }

            // Clear cached references
            originalParents.Clear();
        }
        
    }
}
