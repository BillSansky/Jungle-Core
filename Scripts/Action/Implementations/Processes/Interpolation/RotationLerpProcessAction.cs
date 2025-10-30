using System;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using Jungle.Values.UnityTypes;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo("Interpolates a transform toward a target rotation with curve-based easing.", "d_RotateTool")]
    [Serializable]
    public class RotationLerpProcessAction : LerpProcessAction<Quaternion>
    {
        [SerializeReference][JungleClassSelection] private ITransformValue targetTransform = new TransformLocalValue();
        [SerializeReference][JungleClassSelection] private IVector3Value targetRotation = new Vector3Value(Vector3.zero);
        [SerializeField] private bool useLocalRotation = true;

        private Transform resolvedTransform;

        protected override void OnBeforeStart()
        {
            resolvedTransform = ResolveTargetTransform();
        }

        protected override Quaternion GetStartValue()
        {
            return ReadRotation(resolvedTransform, useLocalRotation);
        }

        protected override Quaternion GetEndValue()
        {
            return Quaternion.Euler(targetRotation.V);
        }

        protected override Quaternion LerpValue(Quaternion start, Quaternion end, float t)
        {
            return Quaternion.LerpUnclamped(start, end, t);
        }

        protected override void ApplyValue(Quaternion value)
        {
            ApplyRotation(resolvedTransform, value, useLocalRotation);
        }

        private Transform ResolveTargetTransform()
        {
            Debug.Assert(targetTransform != null, "Target transform provider has not been assigned.");

            var transform = targetTransform.V;
            Debug.Assert(transform != null, "The transform provider returned a null transform instance.");

            return transform;
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
    }
}
