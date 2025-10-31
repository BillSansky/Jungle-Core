using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Interpolates a Transform scale toward a target size during the process.
    /// </summary>
    [JungleClassInfo("Smoothly scales a transform to a target size over time using an animation curve.", "d_ScaleTool")]
    [Serializable]
    public class ScaleLerpProcessAction : LerpProcessAction<Vector3>
    {
        [SerializeReference] [JungleClassSelection]
        private ITransformValue targetTransform = new TransformLocalValue();

        [SerializeReference] [JungleClassSelection]
        private IVector3Value targetScale = new Vector3Value(Vector3.one);

        private Vector3 originalScale;
        /// <summary>
        /// Handles the OnBeforeStart event.
        /// </summary>
        protected override void OnBeforeStart()
        {
            var transform = targetTransform?.V;
            Debug.Assert(transform != null, "Target transform is null.");
            originalScale = transform.localScale;
        }
        /// <summary>
        /// Supplies the transform's captured starting scale for interpolation.
        /// </summary>
        protected override Vector3 GetStartValue()
        {
            return originalScale;
        }
        /// <summary>
        /// Retrieves the desired scale from the configured value source.
        /// </summary>
        protected override Vector3 GetEndValue()
        {
            return targetScale.V;
        }
        /// <summary>
        /// Blends between the current and target scale using standard Vector3 interpolation.
        /// </summary>
        protected override Vector3 LerpValue(Vector3 start, Vector3 end, float t)
        {
            return Vector3.Lerp(start, end, t);
        }
        /// <summary>
        /// Applies the interpolated scale back onto the transform.
        /// </summary>
        protected override void ApplyValue(Vector3 value)
        {
            var transform = targetTransform.V;
            Debug.Assert(transform != null, "Target transform is null.");
            transform.localScale = value;
        }
    }
}