using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves Transform local position, rotation, and scale.
    /// </summary>
    public class TransformLocalState : SaveState<Transform>
    {
        private Vector3 localPosition;
        private Quaternion localRotation;
        private Vector3 localScale;

        protected override void OnCapture(Transform target)
        {
            localPosition = target.localPosition;
            localRotation = target.localRotation;
            localScale = target.localScale;
        }

        protected override void OnRestore(Transform target)
        {
            target.localPosition = localPosition;
            target.localRotation = localRotation;
            target.localScale = localScale;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with any other Transform state
            return otherState is TransformPositionState ||
                   otherState is TransformRotationState ||
                   otherState is TransformScaleState ||
                   otherState is TransformPositionRotationState ||
                   otherState is TransformFullState;
        }
    }
}