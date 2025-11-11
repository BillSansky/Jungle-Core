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
    public class ParentToContextStateAction : IImmediateAction
    {
        [Header("Target Configuration")] [SerializeField]
        private ITransformValue targetTransforms;

        [Header("Parent Configuration")] [SerializeReference] [JungleClassSelection]
        private ITransformValue parentTransform;

        [Header("Movement Options")] [SerializeField]
        private bool preserveWorldPosition = true;


        // Cache original parents for potential reversion
        private readonly List<Transform> originalParents = new();
        private bool skipStop;
        private bool hasStarted;

        public void Start(Action callback = null)
        {
            if (hasStarted)
            {
                return;
            }

            originalParents.Clear();
            foreach (var target in targetTransforms.Values)
            {
                if (target != null)
                {
                    originalParents.Add(target.parent);
                }
            }

            var parent = parentTransform.V;

            foreach (var t in targetTransforms.Values)
            {
                t.SetParent(parent, preserveWorldPosition);
            }

            hasStarted = true;
            skipStop = false;
            callback?.Invoke();
        }

        public void Stop()
        {
            if (!hasStarted)
            {
                return;
            }

            if (skipStop)
            {
                hasStarted = false;
                return;
            }

            int i = 0;
            foreach (var trans in targetTransforms.Values)
            {
                trans.SetParent(originalParents[i], preserveWorldPosition);
                i++;
            }

            originalParents.Clear();
            hasStarted = false;
        }
    }
}