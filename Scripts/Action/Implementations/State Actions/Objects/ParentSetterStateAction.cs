using System;
using System.Collections.Generic;
using System.Linq;
using Jungle.Actions;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    [JungleClassInfo(
        "Parent Setter Action",
        "Sets the parent of targets when the action starts and optionally restores the original parent on stop.",
        "d_UnityEditor.HierarchyWindow",
        "Actions/State")]
    /// <summary>
    /// Implements the parent setter state action action.
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
        /// Invoked when the state becomes active.
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
        /// Invoked when the state finishes running.
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