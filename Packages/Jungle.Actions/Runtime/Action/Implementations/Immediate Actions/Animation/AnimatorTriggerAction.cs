using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Fires an animator trigger when the action starts and optionally resets it on stop.
    /// </summary>
    [JungleClassInfo("Animator Trigger Action", "Fires an animator trigger when the action starts and optionally resets it on stop.", "d_AnimationClip", "Actions/State")]
    [Serializable]
    public class AnimatorTriggerAction : IImmediateAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeField] private string triggerName = "Activate";
        [SerializeField] private bool resetOnStop = true;

        private Animator cachedAnimator;
        private bool hasExecuted;

        public void Start(Action callback = null)
        {
            var animator = ResolveAnimator();

            if (string.IsNullOrEmpty(triggerName))
            {
                throw new InvalidOperationException("Animator trigger name must be provided before starting the action.");
            }

            animator.SetTrigger(triggerName);

            hasExecuted = true;
            callback?.Invoke();
        }

        public void Stop()
        {
            if (!hasExecuted)
            {
                return;
            }

            if (!resetOnStop)
            {
                hasExecuted = false;
                return;
            }

            var animator = cachedAnimator ?? ResolveAnimator();
            if (!string.IsNullOrEmpty(triggerName))
            {
                animator.ResetTrigger(triggerName);
            }

            hasExecuted = false;
        }

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
