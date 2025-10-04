using System;
using System.Collections;
using Jungle.Actions;
using Jungle.Core;
using Jungle.Transforming;
using Jungle.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Jungle.Actions
{

    [Serializable]
    public class DragZoneTransformerBlendAction : StartStopAction
    {
        private DragZone dragZone;
        [SerializeField] private float transitionDuration = 1f;
        [SerializeField] private AnimationCurve transitionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Position Blend Settings")]
        [SerializeField] private AnimationCurve positionBlendCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] [Range(0f, 1f)] private float positionMinBlendPercent;
        [SerializeField] [Range(0f, 1f)] private float positionMaxBlendPercent = 1f;

        [Header("Rotation Blend Settings")]
        [SerializeField] private AnimationCurve rotationBlendCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] [Range(0f, 1f)] private float rotationMinBlendPercent;
        [SerializeField] [Range(0f, 1f)] private float rotationMaxBlendPercent = 1f;

        [Header("Scale Blend Settings")]
        [SerializeField] private AnimationCurve scaleBlendCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] [Range(0f, 1f)] private float scaleMinBlendPercent;
        [SerializeField] [Range(0f, 1f)] private float scaleMaxBlendPercent = 1f;

        [Header("Blend Target")]
        [SerializeField] private TargetSource targetTargetSource;

        private Coroutine blendCoroutine;

        [FormerlySerializedAs("blendedDragSource")] [FormerlySerializedAs("blendedDragTransformSource")] [SerializeField] private BlendedTargetSource blendedTargetSource;

        private TargetSource originalSource;

        public void StartActionWithContext(DraggableObject draggableInContext, DragZone dragZoneInContext)
        {
            UpdateContext(dragZoneInContext, draggableInContext);

            StartAction();
        }

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
            Debug.Assert(dragZone, "Drag zone is null");
            Debug.Assert(targetTargetSource, "Target drag source is not set");

            if (blendCoroutine != null)
                CleanUpTransition();

            Debug.Assert(dragZone.TargetSource != null,
                "Drag zone DragSourceLogic is null");
            Debug.Assert(dragZone.TargetSource != blendedTargetSource,
                "Drag zone DragSourceLogic cannot be the same as blended provider");

            // If we're already blending, stop the current blend
            if (blendCoroutine != null) CoroutineRunner.StopManagedCoroutine(blendCoroutine);

            // Store the original provider
            originalSource = dragZone.TargetSource;

            // Update the providers
            blendedTargetSource.ProviderA = dragZone.TargetSource;
            blendedTargetSource.ProviderB = targetTargetSource;

            dragZone.TargetSource = blendedTargetSource;
            // Start the blend transition
            blendCoroutine = CoroutineRunner.StartManagedCoroutine(BlendTransition());
        }

        protected override void OnStop()
        {
            if (blendCoroutine != null)
            {
                CoroutineRunner.StopManagedCoroutine(blendCoroutine);
                blendCoroutine = null;
            }

            if (!originalSource || !dragZone)
                return;

            blendCoroutine = CoroutineRunner.StartManagedCoroutine(LerpBackToOriginal());
        }

        public override void OneShot(DraggableObject draggableInContext, DragZone dragZoneInContext)
        {
            UpdateContext(dragZoneInContext, draggableInContext);
            StartAction();
            StopAction();
        }

        private IEnumerator BlendTransition()
        {
            var elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                var normalizedTime = elapsedTime / transitionDuration;
                var curveValue = transitionCurve.Evaluate(normalizedTime);

                // Calculate position blend factor
                float positionCurveValue = positionBlendCurve.Evaluate(curveValue);
                float positionBlend = Mathf.Lerp(positionMinBlendPercent, positionMaxBlendPercent, positionCurveValue);
                blendedTargetSource.PositionBlendFactor = positionBlend;

                // Calculate rotation blend factor
                float rotationCurveValue = rotationBlendCurve.Evaluate(curveValue);
                float rotationBlend = Mathf.Lerp(rotationMinBlendPercent, rotationMaxBlendPercent, rotationCurveValue);
                blendedTargetSource.RotationBlendFactor = rotationBlend;

                // Calculate scale blend factor
                float scaleCurveValue = scaleBlendCurve.Evaluate(curveValue);
                float scaleBlend = Mathf.Lerp(scaleMinBlendPercent, scaleMaxBlendPercent, scaleCurveValue);
                blendedTargetSource.ScaleBlendFactor = scaleBlend;

                yield return null;
            }

            // Set final blend values
            blendedTargetSource.PositionBlendFactor = positionMaxBlendPercent;
            blendedTargetSource.RotationBlendFactor = rotationMaxBlendPercent;
            blendedTargetSource.ScaleBlendFactor = scaleMaxBlendPercent;

            blendCoroutine = null;
        }

        private IEnumerator LerpBackToOriginal()
        {
            var elapsedTime = 0f;
            var startPositionBlend = blendedTargetSource.PositionBlendFactor;
            var startRotationBlend = blendedTargetSource.RotationBlendFactor;
            var startScaleBlend = blendedTargetSource.ScaleBlendFactor;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                var normalizedTime = elapsedTime / transitionDuration;
                var curveValue = transitionCurve.Evaluate(normalizedTime);

                // Calculate position blend factor
                float positionCurveValue = positionBlendCurve.Evaluate(curveValue);
                float positionBlend = Mathf.Lerp(startPositionBlend, positionMinBlendPercent, positionCurveValue);
                blendedTargetSource.PositionBlendFactor = positionBlend;

                // Calculate rotation blend factor
                float rotationCurveValue = rotationBlendCurve.Evaluate(curveValue);
                float rotationBlend = Mathf.Lerp(startRotationBlend, rotationMinBlendPercent, rotationCurveValue);
                blendedTargetSource.RotationBlendFactor = rotationBlend;

                // Calculate scale blend factor
                float scaleCurveValue = scaleBlendCurve.Evaluate(curveValue);
                float scaleBlend = Mathf.Lerp(startScaleBlend, scaleMinBlendPercent, scaleCurveValue);
                blendedTargetSource.ScaleBlendFactor = scaleBlend;

                yield return null;
            }

            // Set final blend values
            blendedTargetSource.PositionBlendFactor = positionMinBlendPercent;
            blendedTargetSource.RotationBlendFactor = rotationMinBlendPercent;
            blendedTargetSource.ScaleBlendFactor = scaleMinBlendPercent;

            dragZone.TargetSource = originalSource;
        }

        public override void UpdateContext(DragZone dragZone, DraggableObject draggable)
        {
            this.dragZone = dragZone;
        }

        private void CleanUpTransition()
        {
            CoroutineRunner.StopManagedCoroutine(blendCoroutine);

            // Reset all blend factors to minimum values
            blendedTargetSource.PositionBlendFactor = positionMinBlendPercent;
            blendedTargetSource.RotationBlendFactor = rotationMinBlendPercent;
            blendedTargetSource.ScaleBlendFactor = scaleMinBlendPercent;

            if (dragZone && originalSource)
                dragZone.TargetSource = originalSource;
        }

    }
}

