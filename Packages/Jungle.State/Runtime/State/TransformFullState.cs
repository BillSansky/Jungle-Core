using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves full Transform state: position, rotation, and scale.
    /// </summary>
    public class TransformFullState : SaveState<Transform>
    {
        private Vector3 position;
        private Quaternion rotation;
        private Vector3 localScale;

        protected override void OnCapture(Transform target)
        {
            position = target.position;
            rotation = target.rotation;
            localScale = target.localScale;
        }

        protected override void OnRestore(Transform target)
        {
            target.position = position;
            target.rotation = rotation;
            target.localScale = localScale;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with any other Transform state
            return otherState is TransformPositionState ||
                   otherState is TransformRotationState ||
                   otherState is TransformScaleState ||
                   otherState is TransformPositionRotationState ||
                   otherState is TransformLocalState;
        }
    }
}