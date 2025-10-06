using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values.GameDev;
using Jungle.Values.Primitives;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Tweens an animator float parameter to a target value and optionally reverts it on stop.", "d_AnimationClip")]
    [Serializable]
    public class AnimatorFloatLerpAction : ProcessAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeReference] private IStringValue parameterName = new StringValue("Blend");
        [SerializeReference] private IFloatValue targetValue = new FloatValue(1f);
        [SerializeReference] private IFloatValue duration = new FloatValue(0.35f);
        [SerializeReference] private IAnimationCurveValue interpolation =
            new AnimationCurveValue(AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
        [SerializeReference] private IBoolValue returnToInitialOnStop = new BoolValue(true);

        private Animator cachedAnimator;
        private float cachedInitialValue;
        private bool hasCachedInitialValue;
        private Coroutine activeRoutine;

        public override bool IsTimed => duration?.V > 0f;
        public override float Duration => duration?.V ?? 0f;

       

        protected override void BeginImpl()
        {
            var animator = ResolveAnimator();
            var parameter = ResolveParameterName();
            ValidateParameter(animator, parameter);

            CacheInitialValue(animator, parameter);

            StopActiveRoutine();

            var totalDuration = duration.V;
            var target = targetValue.V;
            var curve = interpolation.V;

            if (totalDuration <= 0f)
            {
                animator.SetFloat(parameter, target);
                return;
            }

            var start = animator.GetFloat(parameter);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(
                LerpValue(animator, parameter, start, target, totalDuration, curve));
        }

        protected override void CompleteImpl()
        {
            if (!returnToInitialOnStop.V || !hasCachedInitialValue)
            {
                return;
            }

            var animator = cachedAnimator ?? ResolveAnimator();
            var parameter = ResolveParameterName();

            StopActiveRoutine();

            var totalDuration = duration.V;
            var curve = interpolation.V;

            if (totalDuration <= 0f)
            {
                animator.SetFloat(parameter, cachedInitialValue);
                return;
            }

            var start = animator.GetFloat(parameter);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(
                LerpValue(animator, parameter, start, cachedInitialValue, totalDuration, curve));
        }

        private IEnumerator LerpValue(
            Animator animator,
            string parameter,
            float start,
            float end,
            float totalDuration,
            AnimationCurve curve)
        {
            if (totalDuration <= 0f)
            {
                animator.SetFloat(parameter, end);
                yield break;
            }

            float time = 0f;
            var durationClamp = Mathf.Max(0.0001f, totalDuration);

            while (time < durationClamp)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / durationClamp);
                var curved = curve.Evaluate(t);
                animator.SetFloat(parameter, Mathf.LerpUnclamped(start, end, curved));
                yield return null;
            }

            animator.SetFloat(parameter, end);
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

        private void CacheInitialValue(Animator animator, string parameter)
        {
            cachedInitialValue = animator.GetFloat(parameter);
            hasCachedInitialValue = true;
        }

        private void StopActiveRoutine()
        {
            if (activeRoutine == null)
            {
                return;
            }

            CoroutineRunner.StopManagedCoroutine(activeRoutine);
            activeRoutine = null;
        }
    }
}
