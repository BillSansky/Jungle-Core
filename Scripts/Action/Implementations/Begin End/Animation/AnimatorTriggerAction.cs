using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Fires an animator trigger when the action starts and optionally resets it on stop.", "d_AnimationClip")]
    [Serializable]
    public class AnimatorTriggerAction : IBeginEndAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeField] private string triggerName = "Activate";
        [SerializeField] private bool resetOnStop = true;

        private Animator cachedAnimator;

        public void Begin()
        {
            var animator = ResolveAnimator();

            if (string.IsNullOrEmpty(triggerName))
            {
                throw new InvalidOperationException("Animator trigger name must be provided before starting the action.");
            }

            animator.SetTrigger(triggerName);
        }

        public void End()
        {
            if (!resetOnStop)
            {
                return;
            }

            var animator = cachedAnimator ?? ResolveAnimator();
            if (string.IsNullOrEmpty(triggerName))
            {
                return;
            }

            animator.ResetTrigger(triggerName);
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
