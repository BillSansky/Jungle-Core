using System.Collections.Generic;
using System.Linq;
using Jungle.Actions;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    [JungleClassInfo(
        "Sets the parent of targets  when the action starts. Can optionally reset to original parent when stopped.",
        "d_UnityEditor.HierarchyWindow")]
    /// <summary>
    /// Reparents a target Transform to the specified container when the state activates.
    /// </summary>
    public class ParentSetterStateAction : IStateAction
    {
        [SerializeReference][JungleClassSelection] private ITransformValue targetTransforms = new TransformLocalValue();
        [SerializeField] private Transform parentTransform;
        [SerializeField] private bool setParentOnStart = true;
        [SerializeField] private bool resetParentOnStop = true;

        private List<Transform> originalParents;
        private bool skipStop;
        /// <summary>
        /// Optionally moves each target under the configured parent and records its original parent.
        /// </summary>
        public void OnStateEnter()
        {
            if (!setParentOnStart) return;

            originalParents = new List<Transform>();

            foreach (var transform in targetTransforms.Values)
            {
                originalParents.Add(transform.parent);
                transform.SetParent(parentTransform, true);
            }
        }
        /// <summary>
        /// Restores original parenting when the state completes if configured to do so.
        /// </summary>
        public void OnStateExit()
        {
            if (skipStop)
            {
                return;
            }

            if (!resetParentOnStop) return;

            var transforms = targetTransforms.Values.ToList();
            for (int i = 0; i < transforms.Count && i < originalParents.Count; i++)
            {
                transforms[i].SetParent(originalParents[i], true);
            }
        }
    }
}