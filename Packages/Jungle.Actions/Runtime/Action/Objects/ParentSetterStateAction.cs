using System;
using Jungle.Actions;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    [JungleClassInfo(
        "Parent Setter Action",
        "Sets the parent of targets when the action executes.",
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

        public void StartProcess(Action callback = null)
        {
            if (setParentOnStart)
            {
                foreach (var transform in targetTransforms.Values)
                {
                    transform.SetParent(parentTransform, true);
                }
            }

            callback?.Invoke();
        }
    }
}