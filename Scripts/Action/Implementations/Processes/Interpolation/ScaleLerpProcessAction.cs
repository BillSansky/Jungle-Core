using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Smoothly scales a transform to a target size over time using an animation curve.", "d_ScaleTool")]
    [Serializable]
    public class ScaleLerpProcessAction : LerpProcessAction<Vector3>
    {
        [SerializeReference] [JungleClassSelection]
        private ITransformValue targetTransform = new TransformLocalValue();

        [SerializeReference] [JungleClassSelection]
        private IVector3Value targetScale = new Vector3Value(Vector3.one);

        private Vector3 originalScale;

        protected override void OnBeforeStart()
        {
            var transform = targetTransform?.Ref;
            Debug.Assert(transform != null, "Target transform is null.");
            originalScale = transform.localScale;
        }

        protected override Vector3 GetStartValue()
        {
            return originalScale;
        }

        protected override Vector3 GetEndValue()
        {
            return targetScale.V;
        }

        protected override Vector3 LerpValue(Vector3 start, Vector3 end, float t)
        {
            return Vector3.Lerp(start, end, t);
        }

        protected override void ApplyValue(Vector3 value)
        {
            var transform = targetTransform.Ref;
            Debug.Assert(transform != null, "Target transform is null.");
            transform.localScale = value;
        }
    }
}