using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.Primitives;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Smoothly moves a transform toward a target position with optional return on stop.", "d_MoveTool")]
    [Serializable]
    public class PositionLerpProcessAction : LerpProcessAction<Vector3>
    {
        [SerializeReference][JungleClassSelection] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeReference][JungleClassSelection] private IVector3Value targetPosition;
        [SerializeReference] private bool useLocalPosition;
        
        private Transform resolvedTransform;

        protected override void OnBeforeStart()
        {
            resolvedTransform = ResolveTargetTransform();
        }

        protected override void OnInterrupted()
        {
        }

        protected override Vector3 GetStartValue()
        {
            return ReadPosition(resolvedTransform, useLocalPosition);
        }

        protected override Vector3 GetEndValue()
        {
            return targetPosition.V;
        }

        protected override Vector3 LerpValue(Vector3 start, Vector3 end, float t)
        {
            return Vector3.LerpUnclamped(start, end, t);
        }

        protected override void ApplyValue(Vector3 value)
        {
            ApplyPosition(resolvedTransform, value, useLocalPosition);
        }

        private Transform ResolveTargetTransform()
        {
            Debug.Assert(targetTransform != null, "Target transform provider has not been assigned.");

            var transform = targetTransform.Ref;
            Debug.Assert(transform != null, "The transform provider returned a null transform instance.");

            return transform;
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
    }
}
