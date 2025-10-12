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
    public class ScaleLerpProcessAction : IProcessAction
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

        private bool isInProgress;
        private bool hasCompleted;

        // IProcessAction interface implementation
        public event Action OnProcessCompleted;
        public bool HasDefinedDuration => duration?.V > 0f;
        public float Duration => duration?.V ?? 0f;
        public bool IsInProgress => isInProgress;
        public bool HasCompleted => hasCompleted;


        private IEnumerator ScaleCoroutine(Transform transform, float durationValue,
            AnimationCurve curve)
        {
            var start = transform.localScale;

            if (durationValue <= 0f)
            {
                transform.localScale = targetScale.V;
                CompleteProcess();
                yield break;
            }

            while (currentLerpTime < 1.0f)
            {
                currentLerpTime += Time.deltaTime / durationValue;
                float t = curve.Evaluate(Mathf.Clamp01(currentLerpTime));

                transform.localScale = Vector3.Lerp(start, targetScale.V, t);

                yield return null;
            }

            CompleteProcess();
        }

        public void Start()
        {
            var transform = targetTransform?.V;
            if (!transform) return;

            CleanupAction();

            if (!wasInitialized) originalScale = transform.localScale;

            currentLerpTime = 0f;
            isInProgress = true;
            hasCompleted = false;
            wasInitialized = true;

            var target = targetScale.V;
            var durationValue = Mathf.Max(0f, duration.V);
            var curve = scaleCurve.V;

            routine = CoroutineRunner.StartManagedCoroutine(ScaleCoroutine(transform, durationValue, curve));
        }

        public void Interrupt()
        {
            if (!isInProgress) return;

            CleanupAction();
            isInProgress = false;
            hasCompleted = false;
        }

        private void CompleteProcess()
        {
            isInProgress = false;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
        }

        private void CleanupAction()
        {
            CoroutineRunner.StopManagedCoroutine(routine);
        }
    }
}