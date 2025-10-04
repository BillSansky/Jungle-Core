using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Interpolates a transform toward a target rotation with curve-based easing.", "d_RotateTool")]
    [Serializable]
    public class RotationLerpAction : ProcessAction
    {
        [SerializeReference] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeField] private Vector3 targetEulerAngles = Vector3.zero;
        [SerializeField] private bool useLocalRotation = true;
        [SerializeField] private float duration = 0.35f;
        [SerializeField] private AnimationCurve interpolation = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private bool returnToInitialOnStop = true;

        private Quaternion cachedInitialRotation;
        private bool hasCachedInitialRotation;
        private Transform resolvedTransform;
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
            resolvedTransform = ResolveTargetTransform();
            CacheInitialRotation(resolvedTransform);

            StopActiveRoutine();

            var target = Quaternion.Euler(targetEulerAngles);

            if (duration <= 0f)
            {
                ApplyRotation(resolvedTransform, target);
                return;
            }

            var start = ReadRotation(resolvedTransform);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(LerpRotation(start, target));
        }

        protected override void OnStop()
        {
            if (resolvedTransform == null)
            {
                return;
            }

            StopActiveRoutine();

            if (!returnToInitialOnStop || !hasCachedInitialRotation)
            {
                return;
            }

            if (duration <= 0f)
            {
                ApplyRotation(resolvedTransform, cachedInitialRotation);
                return;
            }

            var current = ReadRotation(resolvedTransform);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(LerpRotation(current, cachedInitialRotation));
        }

        private IEnumerator LerpRotation(Quaternion start, Quaternion end)
        {
            var transform = ResolveTargetTransform();
            float time = 0f;
            var totalDuration = Mathf.Max(0.0001f, duration);

            while (time < totalDuration)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / totalDuration);
                var curved = interpolation.Evaluate(t);
                ApplyRotation(transform, Quaternion.LerpUnclamped(start, end, curved));
                yield return null;
            }

            ApplyRotation(transform, end);
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

        private void CacheInitialRotation(Transform transform)
        {
            cachedInitialRotation = ReadRotation(transform);
            hasCachedInitialRotation = true;
        }

        private Quaternion ReadRotation(Transform transform)
        {
            return useLocalRotation ? transform.localRotation : transform.rotation;
        }

        private void ApplyRotation(Transform transform, Quaternion rotation)
        {
            if (useLocalRotation)
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
