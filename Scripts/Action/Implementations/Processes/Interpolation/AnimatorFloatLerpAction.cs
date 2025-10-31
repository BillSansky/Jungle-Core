using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.Primitives;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Tweens an Animator float parameter from its start value to a target over time.
    /// </summary>
    [JungleClassInfo("Tweens an animator float parameter to a target value and optionally reverts it on stop.", "d_AnimationClip")]
    [Serializable]
    public class AnimatorFloatLerpAction : LerpProcessAction<float>, IStateAction
    {
        [SerializeReference][JungleClassSelection] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeReference][JungleClassSelection] private IStringValue parameterName = new StringValue("Blend");
        [SerializeReference][JungleClassSelection] private IFloatValue targetValue = new FloatValue(1f);

        private Animator cachedAnimator;
        private float cachedInitialValue;
        private string cachedParameterName;
        /// <summary>
        /// Starts driving the tween when the parent state becomes active.
        /// </summary>
        public void OnStateEnter()
        {
            Start();
        }
        /// <summary>
        /// Stops the tween when the parent state exits.
        /// </summary>
        public void OnStateExit()
        {
            Interrupt();
        }
        /// <summary>
        /// Caches the animator parameter and its starting value before the tween begins.
        /// </summary>
        protected override void OnBeforeStart()
        {
            cachedAnimator = ResolveAnimator();
            cachedParameterName = ResolveParameterName();
            ValidateParameter(cachedAnimator, cachedParameterName);
            cachedInitialValue = cachedAnimator.GetFloat(cachedParameterName);
        }
        /// <summary>
        /// Reads the animator parameter's current value so interpolation begins from its live state.
        /// </summary>
        protected override float GetStartValue()
        {
            return cachedAnimator.GetFloat(cachedParameterName);
        }
        /// <summary>
        /// Returns the target parameter value that the tween should reach.
        /// </summary>
        protected override float GetEndValue()
        {
            return targetValue.V;
        }
        /// <summary>
        /// Produces a float interpolated toward the end value using unclamped linear interpolation.
        /// </summary>
        protected override float LerpValue(float start, float end, float t)
        {
            return Mathf.LerpUnclamped(start, end, t);
        }
        /// <summary>
        /// Writes the interpolated value back to the animator parameter.
        /// </summary>
        protected override void ApplyValue(float value)
        {
            cachedAnimator.SetFloat(cachedParameterName, value);
        }
        /// <summary>
        /// Restores the original parameter value if the tween is cancelled.
        /// </summary>
        protected override void OnInterrupted()
        {
            if (cachedAnimator != null && !string.IsNullOrEmpty(cachedParameterName))
            {
                cachedAnimator.SetFloat(cachedParameterName, cachedInitialValue);
            }
        }
        /// <summary>
        /// Locates the animator component that should receive the parameter updates.
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
        /// <summary>
        /// Resolves the parameter name string from the value wrapper, defaulting to empty when missing.
        /// </summary>
        private string ResolveParameterName()
        {
            return parameterName?.V ?? string.Empty;
        }
        /// <summary>
        /// Ensures the animator actually exposes the configured float parameter before tweening begins.
        /// </summary>
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
