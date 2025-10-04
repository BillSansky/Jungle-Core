using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Tweens an animator float parameter to a target value and optionally reverts it on stop.", "d_AnimationClip")]
    [Serializable]
    public class AnimatorFloatLerpAction : ProcessAction
    {
        [SerializeReference] private IGameObjectValue targetAnimatorObject = new GameObjectValue();
        [SerializeField] private string parameterName = "Blend";
        [SerializeField] private float targetValue = 1f;
        [SerializeField] private float duration = 0.35f;
        [SerializeField] private AnimationCurve interpolation = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private bool returnToInitialOnStop = true;

        private Animator cachedAnimator;
        private float cachedInitialValue;
        private bool hasCachedInitialValue;
        private Coroutine activeRoutine;

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
            ValidateParameter(animator);

            CacheInitialValue(animator);

            StopActiveRoutine();

            if (duration <= 0f)
            {
                animator.SetFloat(parameterName, targetValue);
                return;
            }

            var start = animator.GetFloat(parameterName);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(LerpValue(animator, start, targetValue));
        }

        protected override void OnStop()
        {
            if (!returnToInitialOnStop || !hasCachedInitialValue)
            {
                return;
            }

            var animator = cachedAnimator ?? ResolveAnimator();

            StopActiveRoutine();

            if (duration <= 0f)
            {
                animator.SetFloat(parameterName, cachedInitialValue);
                return;
            }

            var start = animator.GetFloat(parameterName);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(LerpValue(animator, start, cachedInitialValue));
        }

        private IEnumerator LerpValue(Animator animator, float start, float end)
        {
            float time = 0f;
            var totalDuration = Mathf.Max(0.0001f, duration);

            while (time < totalDuration)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / totalDuration);
                var curved = interpolation.Evaluate(t);
                animator.SetFloat(parameterName, Mathf.LerpUnclamped(start, end, curved));
                yield return null;
            }

            animator.SetFloat(parameterName, end);
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

        private void ValidateParameter(Animator animator)
        {
            if (string.IsNullOrEmpty(parameterName))
            {
                throw new InvalidOperationException("Animator float parameter name must be provided before starting the action.");
            }

            foreach (var parameter in animator.parameters)
            {
                if (parameter.type == AnimatorControllerParameterType.Float && parameter.name == parameterName)
                {
                    return;
                }
            }

            throw new InvalidOperationException($"Animator parameter '{parameterName}' was not found or is not of type float.");
        }

        private void CacheInitialValue(Animator animator)
        {
            cachedInitialValue = animator.GetFloat(parameterName);
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
