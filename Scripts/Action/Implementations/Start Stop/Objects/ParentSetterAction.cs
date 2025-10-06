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
    public class ParentSetterAction : ProcessAction
    {
        [SerializeReference][JungleClassSelection] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeField] private Transform parentTransform;
        [SerializeField] private bool setParentOnStart = true;
        [SerializeField] private bool resetParentOnStop = true;

        private Transform originalParent;
        private bool skipStop;

        public override bool IsTimed => false;
        public override float Duration => 0f;

        
        protected override void BeginImpl()
        {
            if (!setParentOnStart) return;
            
            originalParent = targetTransform.V.parent;
            
            targetTransform.V.SetParent(parentTransform, true);
        }

        protected override void CompleteImpl()
        {
            if (skipStop)
            {
                return;
            }

            if (!resetParentOnStop) return;

            targetTransform.V.SetParent(originalParent, true);
        }
    }
}