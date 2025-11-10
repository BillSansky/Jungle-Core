using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves position and rotation of a Transform.
    /// </summary>
    public class TransformPositionRotationState : SaveState<Transform>
    {
        private Vector3 position;
        private Quaternion rotation;

        protected override void OnCapture(Transform target)
        {
            position = target.position;
            rotation = target.rotation;
        }

        protected override void OnRestore(Transform target)
        {
            target.position = position;
            target.rotation = rotation;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with states that modify position or rotation
            return otherState is TransformPositionState ||
                   otherState is TransformRotationState ||
                   otherState is TransformFullState ||
                   otherState is TransformLocalState;
        }
    }
}