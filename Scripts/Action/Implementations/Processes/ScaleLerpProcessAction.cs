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
    public class ScaleLerpProcessAction : ProcessAction
    {
        [SerializeReference] [JungleClassSelection]
        private ITransformValue targetTransform = new TransformLocalValue();

        [SerializeReference] [JungleClassSelection]
        private IVector3Value targetScale = new Vector3Value(Vector3.one);

        [SerializeReference] [JungleClassSelection]
        private IFloatValue duration = new FloatValue(1f);


        [SerializeReference] [JungleClassSelection]
        private IAnimationCurveValue scaleCurve =
            new AnimationCurveValue(AnimationCurve.EaseInOut(0, 0, 1, 1));

        private Vector3 originalScale;

        private float currentLerpTime;

        private bool wasInitialized;

        private Coroutine routine;

        public override bool IsTimed => duration?.V > 0f;
        public override float Duration => duration?.V ?? 0f;


        private IEnumerator ScaleCoroutine(Transform transform, float durationValue,
            AnimationCurve curve)
        {
            var start = transform.localScale;

            if (durationValue <= 0f)
            {
                transform.localScale = targetScale.V;
                callback.Invoke();
                yield break;
            }

            while (currentLerpTime < 1.0f)
            {
                currentLerpTime += Time.deltaTime / durationValue;
                float t = curve.Evaluate(Mathf.Clamp01(currentLerpTime));

                transform.localScale = Vector3.Lerp(start, targetScale.V, t);

                yield return null;
            }

            callback.Invoke();
        }

        protected override void BeginImpl()
        {
            var transform = targetTransform?.V;
            if (!transform) return;

            CleanupAction();

            if (!wasInitialized) originalScale = transform.localScale;

            currentLerpTime = 0f;

            wasInitialized = true;

            var target = targetScale.V;
            var durationValue = Mathf.Max(0f, duration.V);
            var curve = scaleCurve.V;


            routine = CoroutineRunner.StartManagedCoroutine(ScaleCoroutine(transform, durationValue, curve));
        }

        protected override void InterruptOrCompleteCleanup()
        {
            var transform = targetTransform?.V;
            if (!transform) return;

            CleanupAction();
            
        }

        private Action callback;

        protected override void RegisterInternalCompletionListener(Action onCompleted)
        {
            callback = onCompleted;
        }

        private void CleanupAction()
        {
            CoroutineRunner.StopManagedCoroutine(routine);
        }

        private void OnDisable()
        {
            CleanupAction();
        }
        
    }
}