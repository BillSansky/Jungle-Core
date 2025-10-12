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
    public class PositionLerpProcessAction : IProcessAction
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
        private bool isInProgress;
        private bool hasCompleted;

        public event Action OnProcessCompleted;

        public bool HasDefinedDuration => duration?.V > 0f;
        public float Duration => duration?.V ?? 0f;
        public bool IsInProgress => isInProgress;
        public bool HasCompleted => hasCompleted;

        public void Execute()
        {
            Start();
        }

        public void Start()
        {
            if (isInProgress)
            {
                return;
            }

            isInProgress = true;
            hasCompleted = false;

            resolvedTransform = ResolveTargetTransform();
            var useLocal = useLocalPosition;
            var totalDuration = duration.V;
            var curve = interpolation.V;

            CacheInitialPosition(resolvedTransform, useLocal);

            StopActiveRoutine();

            if (totalDuration <= 0f)
            {
                ApplyPosition(resolvedTransform, targetPosition.V, useLocal);
                Complete();
                return;
            }

            var start = ReadPosition(resolvedTransform, useLocal);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(
                LerpPosition(resolvedTransform, start, totalDuration, curve, useLocal));
        }

        public void Interrupt()
        {
            if (!isInProgress)
            {
                return;
            }

            if (resolvedTransform == null)
            {
                isInProgress = false;
                hasCompleted = false;
                return;
            }

            StopActiveRoutine();

            if (!returnToInitialOnStop || !hasCachedInitialPosition)
            {
                isInProgress = false;
                hasCompleted = false;
                return;
            }

            var useLocal = useLocalPosition;
            var totalDuration = duration.V;
            var curve = interpolation.V;

            if (totalDuration <= 0f)
            {
                ApplyPosition(resolvedTransform, cachedInitialPosition, useLocal);
                isInProgress = false;
                hasCompleted = false;
                return;
            }

            var current = ReadPosition(resolvedTransform, useLocal);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(
                LerpToInitialPosition(resolvedTransform, current, totalDuration, curve, useLocal));
        }

        private void Complete()
        {
            if (!isInProgress)
            {
                return;
            }

            isInProgress = false;
            hasCompleted = true;
            OnProcessCompleted?.Invoke();
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
                Complete();
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
            Complete();
        }

        private IEnumerator LerpToInitialPosition(
            Transform transform,
            Vector3 start,
            float totalDuration,
            AnimationCurve curve,
            bool useLocal)
        {
            if (totalDuration <= 0f)
            {
                ApplyPosition(transform, cachedInitialPosition, useLocal);
                isInProgress = false;
                hasCompleted = false;
                yield break;
            }

            float time = 0f;
            var durationClamp = Mathf.Max(0.0001f, totalDuration);

            while (time < durationClamp)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / durationClamp);
                var curved = curve.Evaluate(t);
                ApplyPosition(transform, Vector3.LerpUnclamped(start, cachedInitialPosition, curved), useLocal);
                yield return null;
            }

            ApplyPosition(transform, cachedInitialPosition, useLocal);
            isInProgress = false;
            hasCompleted = false;
        }

        private Transform ResolveTargetTransform()
        {
            Debug.Assert(targetTransform != null, "Target transform provider has not been assigned.");

            var transform = targetTransform.V;
            Debug.Assert(transform != null, "The transform provider returned a null transform instance.");

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
