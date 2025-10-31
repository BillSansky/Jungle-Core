using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Sets or resets Animator trigger parameters as part of the state change.
    /// </summary>
    [JungleClassInfo("Fires an animator trigger when the action starts and optionally resets it on stop.", "d_AnimationClip")]
    [Serializable]
    public class AnimatorTriggerAction : IStateAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeField] private string triggerName = "Activate";
        [SerializeField] private bool resetOnStop = true;

        private Animator cachedAnimator;
        /// <summary>
        /// Fires the trigger when the state starts so the animator can play the intended transition.
        /// </summary>
        public void OnStateEnter()
        {
            var animator = ResolveAnimator();

            if (string.IsNullOrEmpty(triggerName))
            {
                throw new InvalidOperationException("Animator trigger name must be provided before starting the action.");
            }

            animator.SetTrigger(triggerName);
        }
        /// <summary>
        /// Optionally resets the trigger when the state stops to keep animator state predictable.
        /// </summary>
        public void OnStateExit()
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
        /// <summary>
        /// Finds and caches the animator that should receive trigger commands.
        /// </summary>
        private Animator ResolveAnimator()
        {
            if (targetAnimatorObject == null)
            {
                throw new InvalidOperationException("Animator GameObject provider has not been assigned.");
            }

            var gameObject = targetAnimatorObject.G;
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
