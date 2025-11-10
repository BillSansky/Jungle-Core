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
    public class ParentSetterStateAction : IImmediateAction
    {
        [SerializeReference][JungleClassSelection] private ITransformValue targetTransforms = new TransformLocalValue();
        [SerializeField] private Transform parentTransform;
        [SerializeField] private bool setParentOnStart = true;
        [SerializeField] private bool resetParentOnStop = true;

        private List<Transform> originalParents;
        private bool skipStop;
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

            if (setParentOnStart)
            {
                originalParents = new List<Transform>();

                foreach (var transform in targetTransforms.Values)
                {
                    originalParents.Add(transform.parent);
                    transform.SetParent(parentTransform, true);
                }
            }

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

            if (skipStop)
            {
                isInProgress = false;
                hasCompleted = false;
                return;
            }

            if (resetParentOnStop && originalParents != null)
            {
                var transforms = targetTransforms.Values.ToList();
                for (int i = 0; i < transforms.Count && i < originalParents.Count; i++)
                {
                    transforms[i].SetParent(originalParents[i], true);
                }
            }

            isInProgress = false;
            hasCompleted = false;
        }
    }
}