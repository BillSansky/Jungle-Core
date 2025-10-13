using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Tweens an animator float parameter to a target value and optionally reverts it on stop.", "d_AnimationClip")]
    [Serializable]
    public class AnimatorFloatLerpAction : LerpProcessAction<float>, IStateAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeReference] private IStringValue parameterName = new StringValue("Blend");
        [SerializeReference] private IFloatValue targetValue = new FloatValue(1f);

        private Animator cachedAnimator;
        private float cachedInitialValue;
        private string cachedParameterName;

        public void OnStateEnter()
        {
            Start();
        }

        public void OnStateExit()
        {
            Interrupt();
        }

        protected override void OnBeforeStart()
        {
            cachedAnimator = ResolveAnimator();
            cachedParameterName = ResolveParameterName();
            ValidateParameter(cachedAnimator, cachedParameterName);
            cachedInitialValue = cachedAnimator.GetFloat(cachedParameterName);
        }

        protected override float GetStartValue()
        {
            return cachedAnimator.GetFloat(cachedParameterName);
        }

        protected override float GetEndValue()
        {
            return targetValue.V;
        }

        protected override float LerpValue(float start, float end, float t)
        {
            return Mathf.LerpUnclamped(start, end, t);
        }

        protected override void ApplyValue(float value)
        {
            cachedAnimator.SetFloat(cachedParameterName, value);
        }

        protected override void OnInterrupted()
        {
            if (cachedAnimator != null && !string.IsNullOrEmpty(cachedParameterName))
            {
                cachedAnimator.SetFloat(cachedParameterName, cachedInitialValue);
            }
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

        private string ResolveParameterName()
        {
            return parameterName?.V ?? string.Empty;
        }

        private void ValidateParameter(Animator animator, string parameter)
        {
            if (string.IsNullOrEmpty(parameter))
            {
                throw new InvalidOperationException("Animator float parameter name must be provided before starting the action.");
            }

            foreach (var animatorParameter in animator.parameters)
            {
                if (animatorParameter.type == AnimatorControllerParameterType.Float && animatorParameter.name == parameter)
                {
                    return;
                }
            }

            throw new InvalidOperationException($"Animator parameter '{parameter}' was not found or is not of type float.");
        }

    }
}
