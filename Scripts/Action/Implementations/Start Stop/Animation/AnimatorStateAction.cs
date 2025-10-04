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
    public class AnimatorStateAction : ProcessAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeField] private string stateName = "StateName";
        [SerializeField] private int layerIndex;
        [SerializeField] private bool startFromBeginning = true;
        [SerializeField] private float startNormalizedTime = 0f;
        [SerializeField] private bool restorePreviousStateOnStop = true;

        private Animator cachedAnimator;
        private int previousStateHash;
        private float previousNormalizedTime;
        private bool hasPreviousState;

        public override bool IsTimed => false;
        public override float Duration => 0f;

        public void StartAction()
        {
            Start();
        }

        public void StopAction()
        {
            Stop();
        }

        protected override void OnStart()
        {
            var animator = ResolveAnimator();

            if (string.IsNullOrWhiteSpace(stateName))
            {
                throw new InvalidOperationException("Animator state name must be provided before starting the action.");
            }

            if (restorePreviousStateOnStop)
            {
                var stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
                previousStateHash = stateInfo.fullPathHash;
                previousNormalizedTime = stateInfo.normalizedTime;
                hasPreviousState = true;
            }
            else
            {
                hasPreviousState = false;
            }

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

        protected override void OnStop()
        {
            if (!restorePreviousStateOnStop || !hasPreviousState)
            {
                return;
            }

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
