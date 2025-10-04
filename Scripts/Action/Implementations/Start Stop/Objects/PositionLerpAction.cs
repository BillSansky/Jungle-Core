using System;
using System.Collections;
using Jungle.Attributes;
using Jungle.Utils;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Smoothly moves a transform toward a target position with optional return on stop.", "d_MoveTool")]
    [Serializable]
    public class PositionLerpAction : ProcessAction
    {
        [SerializeReference] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeField] private Vector3 targetPosition = Vector3.zero;
        [SerializeField] private bool useLocalPosition = true;
        [SerializeField] private float duration = 0.35f;
        [SerializeField] private AnimationCurve interpolation = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
        [SerializeField] private bool returnToInitialOnStop = true;

        private Vector3 cachedInitialPosition;
        private bool hasCachedInitialPosition;
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
            CacheInitialPosition(resolvedTransform);

            StopActiveRoutine();

            if (duration <= 0f)
            {
                ApplyPosition(resolvedTransform, targetPosition);
                return;
            }

            var start = ReadPosition(resolvedTransform);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(LerpPosition(start, targetPosition));
        }

        protected override void OnStop()
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

            if (duration <= 0f)
            {
                ApplyPosition(resolvedTransform, cachedInitialPosition);
                return;
            }

            var current = ReadPosition(resolvedTransform);
            activeRoutine = CoroutineRunner.StartManagedCoroutine(LerpPosition(current, cachedInitialPosition));
        }

        private IEnumerator LerpPosition(Vector3 start, Vector3 end)
        {
            var transform = ResolveTargetTransform();
            float time = 0f;
            var totalDuration = Mathf.Max(0.0001f, duration);

            while (time < totalDuration)
            {
                time += Time.deltaTime;
                var t = Mathf.Clamp01(time / totalDuration);
                var curved = interpolation.Evaluate(t);
                ApplyPosition(transform, Vector3.LerpUnclamped(start, end, curved));
                yield return null;
            }

            ApplyPosition(transform, end);
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

        private void CacheInitialPosition(Transform transform)
        {
            cachedInitialPosition = ReadPosition(transform);
            hasCachedInitialPosition = true;
        }

        private Vector3 ReadPosition(Transform transform)
        {
            return useLocalPosition ? transform.localPosition : transform.position;
        }

        private void ApplyPosition(Transform transform, Vector3 position)
        {
            if (useLocalPosition)
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
