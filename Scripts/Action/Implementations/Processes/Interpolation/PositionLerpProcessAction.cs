using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.Primitives;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Interpolates a Transform position toward a target location during the process.
    /// </summary>
    [JungleClassInfo("Smoothly moves a transform toward a target position with optional return on stop.", "d_MoveTool")]
    [Serializable]
    public class PositionLerpProcessAction : LerpProcessAction<Vector3>
    {
        [SerializeReference][JungleClassSelection] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeReference][JungleClassSelection] private IVector3Value targetPosition;
        [SerializeReference] private bool useLocalPosition;
        
        private Transform resolvedTransform;
        /// <summary>
        /// Handles the OnBeforeStart event.
        /// </summary>
        protected override void OnBeforeStart()
        {
            resolvedTransform = ResolveTargetTransform();
        }
        /// <summary>
        /// Handles the OnInterrupted event.
        /// </summary>
        protected override void OnInterrupted()
        {
        }
        /// <summary>
        /// Captures the transform's current position in either world or local space as the interpolation baseline.
        /// </summary>
        protected override Vector3 GetStartValue()
        {
            return ReadPosition(resolvedTransform, useLocalPosition);
        }
        /// <summary>
        /// Returns the destination position supplied by the configured value provider.
        /// </summary>
        protected override Vector3 GetEndValue()
        {
            return targetPosition.V;
        }
        /// <summary>
        /// Calculates a Vector3 between the start and end using unclamped linear interpolation.
        /// </summary>
        protected override Vector3 LerpValue(Vector3 start, Vector3 end, float t)
        {
            return Vector3.LerpUnclamped(start, end, t);
        }
        /// <summary>
        /// Writes the interpolated position back to the transform in the requested space.
        /// </summary>
        protected override void ApplyValue(Vector3 value)
        {
            ApplyPosition(resolvedTransform, value, useLocalPosition);
        }
        /// <summary>
        /// Resolves which transform should be moved during the interpolation.
        /// </summary>
        private Transform ResolveTargetTransform()
        {
            Debug.Assert(targetTransform != null, "Target transform provider has not been assigned.");

            var transform = targetTransform.V;
            Debug.Assert(transform != null, "The transform provider returned a null transform instance.");

            return transform;
        }
        /// <summary>
        /// Retrieves the transform's current position, respecting the local-space flag.
        /// </summary>
        private Vector3 ReadPosition(Transform transform, bool useLocal)
        {
            return useLocal ? transform.localPosition : transform.position;
        }
        /// <summary>
        /// Assigns the provided position to the transform, honoring the local-space setting.
        /// </summary>
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
    }
}
