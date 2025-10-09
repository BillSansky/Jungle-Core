using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.Primitives;
using Jungle.Values.UnityTypes;
using Jungle.Utils;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Smoothly scales a transform to a target size over time using an animation curve.", "d_ScaleTool")]
    [Serializable]
    public class ScaleLerpAction : ProcessAction
    {
        [SerializeReference] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeReference] private IVector3Value targetScale = new Vector3Value(Vector3.one);
        [SerializeReference] private IFloatValue duration = new FloatValue(1f);
        [SerializeReference] private IBoolValue lerpBackToInitialValueOnStop = new BoolValue(true);
        [SerializeReference] private IAnimationCurveValue scaleCurve =
            new AnimationCurveValue(AnimationCurve.EaseInOut(0, 0, 1, 1));

        private Vector3 originalScale;

        private float currentLerpTime;
        private bool isLerping;
        private bool wasInitialized;

        private Coroutine routine;

        public override bool IsTimed => duration?.V > 0f;
        public override float Duration => duration?.V ?? 0f;
    
       

        private IEnumerator ScaleCoroutine(Transform transform, Vector3 endScale, float durationValue, AnimationCurve curve)
        {
            var start = transform.localScale;

            if (durationValue <= 0f)
            {
                transform.localScale = endScale;
                isLerping = false;
                yield break;
            }

            while (currentLerpTime < 1.0f)
            {
                currentLerpTime += Time.deltaTime / durationValue;
                float t = curve.Evaluate(Mathf.Clamp01(currentLerpTime));

                transform.localScale = Vector3.Lerp(start, endScale, t);

                yield return null;
            }

            isLerping = false;
        }

        protected override void BeginImpl()
        {
            var transform = targetTransform?.V;
            if (!transform) return;

            CleanupAction();

            if (!wasInitialized) originalScale = transform.localScale;

            currentLerpTime = 0f;
            isLerping = true;
            wasInitialized = true;

            var target = targetScale.V;
            var durationValue = Mathf.Max(0f, duration.V);
            var curve = scaleCurve.V;

            if (durationValue <= 0f)
            {
                transform.localScale = target;
                isLerping = false;
                return;
            }

            routine = CoroutineRunner.StartManagedCoroutine(ScaleCoroutine(transform, target, durationValue, curve));
        }

        protected override void CompleteImpl()
        {
            var transform = targetTransform?.V;
            if (!transform) return;

            CleanupAction();

            if (lerpBackToInitialValueOnStop.V)
            {
                currentLerpTime = 0f;
                isLerping = true;

                CoroutineRunner.StartManagedCoroutine(
                    RevertScaleCoroutine(transform, Mathf.Max(0f, duration.V), scaleCurve.V));
            }
            else
            {
                isLerping = false;
                transform.localScale = originalScale;
            }
        }

        private void CleanupAction()
        {
            CoroutineRunner.StopManagedCoroutine(routine);

            isLerping = false;
        }

        private void OnDisable()
        {
            CleanupAction();
        }

       

       

        private IEnumerator RevertScaleCoroutine(Transform transform, float durationValue, AnimationCurve curve)
        {
            var target = originalScale;
            var start = transform.localScale;

            if (durationValue <= 0f)
            {
                transform.localScale = target;
                isLerping = false;
                yield break;
            }

            while (currentLerpTime < 1.0f)
            {
                currentLerpTime += Time.deltaTime / durationValue;
                float t = curve.Evaluate(Mathf.Clamp01(currentLerpTime));

                transform.localScale = Vector3.Lerp(start, target, t);

                yield return null;
            }

            isLerping = false;
        }

    }
}
