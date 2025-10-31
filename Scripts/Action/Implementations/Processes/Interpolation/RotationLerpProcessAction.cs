using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Interpolates a Transform rotation toward a target orientation over time.
    /// </summary>
    [JungleClassInfo("Interpolates a transform toward a target rotation with curve-based easing.", "d_RotateTool")]
    [Serializable]
    public class RotationLerpProcessAction : LerpProcessAction<Quaternion>
    {
        [SerializeReference][JungleClassSelection] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeReference][JungleClassSelection] private IVector3Value targetRotation = new Vector3Value(Vector3.zero);
        [SerializeField] private bool useLocalRotation = true;

        private Transform resolvedTransform;
        /// <summary>
        /// Resolves the transform reference before interpolation begins.
        /// </summary>
        protected override void OnBeforeStart()
        {
            resolvedTransform = ResolveTargetTransform();
        }
        /// <summary>
        /// Reads the starting rotation from the resolved transform using the configured space.
        /// </summary>
        protected override Quaternion GetStartValue()
        {
            return ReadRotation(resolvedTransform, useLocalRotation);
        }
        /// <summary>
        /// Creates the goal rotation quaternion from the configured target Euler angles.
        /// </summary>
        protected override Quaternion GetEndValue()
        {
            return Quaternion.Euler(targetRotation.V);
        }
        /// <summary>
        /// Produces an interpolated rotation between the start and end quaternions for the current progress value.
        /// </summary>
        protected override Quaternion LerpValue(Quaternion start, Quaternion end, float t)
        {
            return Quaternion.LerpUnclamped(start, end, t);
        }
        /// <summary>
        /// Applies the interpolated rotation back onto the transform in either local or world space.
        /// </summary>
        protected override void ApplyValue(Quaternion value)
        {
            ApplyRotation(resolvedTransform, value, useLocalRotation);
        }
        /// <summary>
        /// Retrieves the transform instance that should be rotated.
        /// </summary>
        private Transform ResolveTargetTransform()
        {
            Debug.Assert(targetTransform != null, "Target transform provider has not been assigned.");

            var transform = targetTransform.V;
            Debug.Assert(transform != null, "The transform provider returned a null transform instance.");

            return transform;
        }
        /// <summary>
        /// Returns the transform's rotation, choosing local or global coordinates based on configuration.
        /// </summary>
        private Quaternion ReadRotation(Transform transform, bool useLocal)
        {
            return useLocal ? transform.localRotation : transform.rotation;
        }
        /// <summary>
        /// Writes the provided rotation to the transform in the appropriate coordinate space.
        /// </summary>
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
    }
}
