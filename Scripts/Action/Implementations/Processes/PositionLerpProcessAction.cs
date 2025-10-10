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
    [JungleClassInfo("Smoothly moves a transform toward a target position with optional return on stop.", "d_MoveTool")]
    [Serializable]
    public class PositionLerpProcessAction : ProcessAction
    {
        [SerializeReference][JungleClassSelection] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeReference][JungleClassSelection] private IVector3Value targetPosition;
        [SerializeReference] private bool useLocalPosition ;
        [SerializeReference] private IFloatValue duration = new FloatValue(0.35f);
        [SerializeReference] private IAnimationCurveValue interpolation =
            new AnimationCurveValue(AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
        [SerializeReference] private bool returnToInitialOnStop;

        private Vector3 cachedInitialPosition;
        private bool hasCachedInitialPosition;
        private Transform resolvedTransform;
        private Coroutine activeRoutine;

        public override bool IsTimed => duration?.V > 0f;
        public override float Duration => duration?.V ?? 0f;

      

        protected override void BeginImpl()
        {
            resolvedTransform = ResolveTargetTransform();
            var useLocal = useLocalPosition;
            var totalDuration = duration.V;
            var curve = interpolation.V;

            

            CacheInitialPosition(resolvedTransform, useLocal);

            StopActiveRoutine();

            if (totalDuration <= 0f)
            {
                ApplyPosition(resolvedTransform, targetPosition.Value(), useLocal);
                return;
            }

            var start = ReadPosition(resolvedTransform, useLocal);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(
                LerpPosition(resolvedTransform, start, totalDuration, curve, useLocal));
        }

        protected override void InterruptOrCompleteCleanup()
        {
            if (resolvedTransform == null)
            {
                return;
            }

            StopActiveRoutine();

            if (!returnToInitialOnStop || !hasCachedInitialPosition)
            {
                return;
            }

            var useLocal = useLocalPosition;
            var totalDuration = duration.V;
            var curve = interpolation.V;

            if (totalDuration <= 0f)
            {
                ApplyPosition(resolvedTransform, cachedInitialPosition, useLocal);
                return;
            }

            var current = ReadPosition(resolvedTransform, useLocal);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(
                LerpPosition(resolvedTransform, current, totalDuration, curve, useLocal));
        }

        private Action callback;
        
        protected override void RegisterInternalCompletionListener(Action onCompleted)
        {
           callback=onCompleted;
        }

        private IEnumerator LerpPosition(
            Transform transform,
            Vector3 start,
            float totalDuration,
            AnimationCurve curve,
            bool useLocal)
        {
            if (totalDuration <= 0f)
            {
                ApplyPosition(transform, targetPosition.V, useLocal);
                yield break;
            }

            float time = 0f;
            var durationClamp = Mathf.Max(0.0001f, totalDuration);

            while (time < durationClamp)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / durationClamp);
                var curved = curve.Evaluate(t);
                ApplyPosition(transform, Vector3.LerpUnclamped(start, targetPosition.V, curved), useLocal);
                yield return null;
            }

            ApplyPosition(transform, targetPosition.V, useLocal);
            callback.Invoke();
        }

        private Transform ResolveTargetTransform()
        {
            if (targetTransform == null)
            {
                throw new InvalidOperationException("Target transform provider has not been assigned.");
            }

            var transform = targetTransform.V;
            if (transform == null)
            {
                throw new InvalidOperationException("The transform provider returned a null transform instance.");
            }

            return transform;
        }

        private void CacheInitialPosition(Transform transform, bool useLocal)
        {
            cachedInitialPosition = ReadPosition(transform, useLocal);
            hasCachedInitialPosition = true;
        }

        private Vector3 ReadPosition(Transform transform, bool useLocal)
        {
            return useLocal ? transform.localPosition : transform.position;
        }

        private void ApplyPosition(Transform transform, Vector3 position, bool useLocal)
        {
            if (useLocal)
            {
                transform.localPosition = position;
            }
            else
            {
                transform.position = position;
            }
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
