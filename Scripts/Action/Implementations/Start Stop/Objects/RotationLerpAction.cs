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
    [JungleClassInfo("Interpolates a transform toward a target rotation with curve-based easing.", "d_RotateTool")]
    [Serializable]
    public class RotationLerpAction : BeginCompleteAction
    {
        [SerializeReference] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeReference] private IVector3Value targetEulerAngles = new Vector3Value(Vector3.zero);
        [SerializeReference] private IBoolValue useLocalRotation = new BoolValue(true);
        [SerializeReference] private IFloatValue duration = new FloatValue(0.35f);
        [SerializeReference] private IAnimationCurveValue interpolation =
            new AnimationCurveValue(AnimationCurve.EaseInOut(0f, 0f, 1f, 1f));
        [SerializeReference] private IBoolValue returnToInitialOnStop = new BoolValue(true);

        private Quaternion cachedInitialRotation;
        private bool hasCachedInitialRotation;
        private Transform resolvedTransform;
        private Coroutine activeRoutine;

        public override bool IsTimed => duration?.V > 0f;
        public override float Duration => duration?.V ?? 0f;

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
            resolvedTransform = ResolveTargetTransform();
            var target = Quaternion.Euler(targetEulerAngles.V);
            var totalDuration = duration.V;
            var curve = interpolation.V;
            var useLocal = useLocalRotation.V;

            CacheInitialRotation(resolvedTransform, useLocal);

            StopActiveRoutine();

            if (totalDuration <= 0f)
            {
                ApplyRotation(resolvedTransform, target, useLocal);
                return;
            }

            var start = ReadRotation(resolvedTransform, useLocal);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(
                LerpRotation(resolvedTransform, start, target, totalDuration, curve, useLocal));
        }

        protected override void OnStop()
        {
            if (resolvedTransform == null)
            {
                return;
            }

            StopActiveRoutine();

            if (!returnToInitialOnStop.V || !hasCachedInitialRotation)
            {
                return;
            }

            var useLocal = useLocalRotation.V;
            var totalDuration = duration.V;
            var curve = interpolation.V;

            if (totalDuration <= 0f)
            {
                ApplyRotation(resolvedTransform, cachedInitialRotation, useLocal);
                return;
            }

            var current = ReadRotation(resolvedTransform, useLocal);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(
                LerpRotation(resolvedTransform, current, cachedInitialRotation, totalDuration, curve, useLocal));
        }

        private IEnumerator LerpRotation(
            Transform transform,
            Quaternion start,
            Quaternion end,
            float totalDuration,
            AnimationCurve curve,
            bool useLocal)
        {
            if (totalDuration <= 0f)
            {
                ApplyRotation(transform, end, useLocal);
                yield break;
            }

            float time = 0f;
            var durationClamp = Mathf.Max(0.0001f, totalDuration);

            while (time < durationClamp)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / durationClamp);
                var curved = curve.Evaluate(t);
                ApplyRotation(transform, Quaternion.LerpUnclamped(start, end, curved), useLocal);
                yield return null;
            }

            ApplyRotation(transform, end, useLocal);
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

        private void CacheInitialRotation(Transform transform, bool useLocal)
        {
            cachedInitialRotation = ReadRotation(transform, useLocal);
            hasCachedInitialRotation = true;
        }

        private Quaternion ReadRotation(Transform transform, bool useLocal)
        {
            return useLocal ? transform.localRotation : transform.rotation;
        }

        private void ApplyRotation(Transform transform, Quaternion rotation, bool useLocal)
        {
            if (useLocal)
            {
                transform.localRotation = rotation;
            }
            else
            {
                transform.rotation = rotation;
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
