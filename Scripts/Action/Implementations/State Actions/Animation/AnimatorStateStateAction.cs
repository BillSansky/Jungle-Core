using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Animator State Action",
        "Plays a specific animator state when the action starts and optionally restores the previous state on stop.",
        "d_AnimationClip",
        "Actions/State")]
    [Serializable]
    public class AnimatorStateStateAction : IStateAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeField] private IStringValue stateName = new StringValue("StateName");
        [SerializeField] private IIntValue layerIndex = new IntValue(0);
        [SerializeField] private bool startFromBeginning = true;
        [SerializeField] private float startNormalizedTime = 0f;


        private Animator cachedAnimator;
        private int previousStateHash;
        private float previousNormalizedTime;


        public void OnStateEnter()
        {
            var animator = ResolveAnimator();

            Debug.Assert(animator != null, "Animator object has not been assigned.");
            Debug.Assert(layerIndex.V >= 0, "Layer index must be greater than or equal to 0.");
            Debug.Assert(layerIndex.V < animator.layerCount, "Layer index must be less than the number of layers.");
            Debug.Assert(!string.IsNullOrEmpty(stateName.V), "State name must be provided.");
            
            var stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex.V);
            previousStateHash = stateInfo.fullPathHash;
            previousNormalizedTime = stateInfo.normalizedTime;
  


            var targetStateHash = Animator.StringToHash(stateName.V);

            if (startFromBeginning)
            {
                animator.Play(targetStateHash, layerIndex.V, Mathf.Clamp01(startNormalizedTime));
            }
            else
            {
                animator.Play(targetStateHash, layerIndex.V);
            }
        }

        public void OnStateExit()
        {
            var animator = cachedAnimator ?? ResolveAnimator();
            var normalizedTime = Mathf.Repeat(previousNormalizedTime, 1f);
            animator.Play(previousStateHash, layerIndex.V, normalizedTime);
        }

        private Animator ResolveAnimator()
        {
            var gameObject = targetAnimatorObject.G;
            cachedAnimator = gameObject.GetComponent<Animator>();

            return cachedAnimator;
        }
    }
}