using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Plays a specific animator state when the action starts and optionally restores the previous state on stop.",
        "d_AnimationClip")]
    [Serializable]
    public class AnimatorStateStateAction : IStateAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeField] private string stateName = "StateName";
        [SerializeField] private int layerIndex;
        [SerializeField] private bool startFromBeginning = true;
        [SerializeField] private float startNormalizedTime = 0f;
      

        private Animator cachedAnimator;
        private int previousStateHash;
        private float previousNormalizedTime;
        private bool hasPreviousState;

        public void OnStateEnter()
        {
            var animator = ResolveAnimator();

            if (string.IsNullOrWhiteSpace(stateName))
            {
                throw new InvalidOperationException("Animator state name must be provided before starting the action.");
            }

           
                var stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
                previousStateHash = stateInfo.fullPathHash;
                previousNormalizedTime = stateInfo.normalizedTime;
                hasPreviousState = true;
            

            var targetStateHash = Animator.StringToHash(stateName);

            if (startFromBeginning)
            {
                animator.Play(targetStateHash, layerIndex, Mathf.Clamp01(startNormalizedTime));
            }
            else
            {
                animator.Play(targetStateHash, layerIndex);
            }
        }

        public void OnStateExit()
        {

            var animator = cachedAnimator ?? ResolveAnimator();
            var normalizedTime = Mathf.Repeat(previousNormalizedTime, 1f);
            animator.Play(previousStateHash, layerIndex, normalizedTime);
        }

        private Animator ResolveAnimator()
        {
            if (targetAnimatorObject == null)
            {
                throw new InvalidOperationException("Animator GameObject provider has not been assigned.");
            }

            var gameObject = targetAnimatorObject.V;
            if (gameObject == null)
            {
                throw new InvalidOperationException("The animator GameObject provider returned a null instance.");
            }

            cachedAnimator = gameObject.GetComponent<Animator>();
            if (cachedAnimator == null)
            {
                throw new InvalidOperationException($"Animator component was not found on '{gameObject.name}'.");
            }

            return cachedAnimator;
        }
    }
}
