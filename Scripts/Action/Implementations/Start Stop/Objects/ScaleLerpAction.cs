using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Values.GameDev;

using Jungle.Utils;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Smoothly scales a transform to a target size over time using an animation curve.", "d_ScaleTool")]
    [Serializable]
    public class ScaleLerpAction : ProcessAction
    {
        [SerializeReference] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeField] private Vector3 targetScale = Vector3.one;
        [SerializeField] private float duration = 1f;
        [SerializeField] private bool LerpBackToInitialValueOnStop = true;
        [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        private Vector3 originalScale;

        private float currentLerpTime;
        private bool isLerping;
        private bool wasInitialized;

        private Coroutine routine;

    
        public void StartAction()
        {
            Start();
        }

        public void StopAction()
        {
            Stop();
        }

        private IEnumerator ScaleCoroutine(Transform transform)
        {
            var start = transform.localScale;
            var end = targetScale;

            while (currentLerpTime < 1.0f)
            {
                currentLerpTime += Time.deltaTime / duration;
                float t = scaleCurve.Evaluate(Mathf.Clamp01(currentLerpTime));

                transform.localScale = Vector3.Lerp(start, end, t);

                yield return null;
            }

            isLerping = false;
        }

        protected override void OnStart()
        {
            var transform = (Transform)targetTransform;
            if (!transform) return;

            CleanupAction();

            if (!wasInitialized) originalScale = transform.localScale;

            currentLerpTime = 0f;
            isLerping = true;
            wasInitialized = true;

            routine = CoroutineRunner.StartManagedCoroutine(ScaleCoroutine(transform));
        }

        protected override void OnStop()
        {
            var transform = (Transform)targetTransform;
            if (!transform) return;

            CleanupAction();

            if (LerpBackToInitialValueOnStop)
            {
                currentLerpTime = 0f;
                isLerping = true;

                CoroutineRunner.StartManagedCoroutine(RevertScaleCoroutine(transform));
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

       

        private IEnumerator OneShotRoutine()
        {
            StartAction();
            while (isLerping)
            {
                yield return null;
            }

            StopAction();
        }

        private IEnumerator RevertScaleCoroutine(Transform transform)
        {
            var target = originalScale;
            var start = transform.localScale;

            while (currentLerpTime < 1.0f)
            {
                currentLerpTime += Time.deltaTime / duration;
                float t = scaleCurve.Evaluate(Mathf.Clamp01(currentLerpTime));

                transform.localScale = Vector3.Lerp(start, target, t);

                yield return null;
            }

            isLerping = false;
        }

    }
}
