using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves only the rotation of a Transform.
    /// </summary>
    public class TransformRotationState : SaveState<Transform>
    {
        private Quaternion rotation;

        protected override void OnCapture(Transform target)
        {
            rotation = target.rotation;
        }

        protected override void OnRestore(Transform target)
        {
            target.rotation = rotation;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with any other Transform state that modifies rotation
            return otherState is TransformPositionRotationState ||
                   otherState is TransformFullState ||
                   otherState is TransformLocalState;
        }
    }
}