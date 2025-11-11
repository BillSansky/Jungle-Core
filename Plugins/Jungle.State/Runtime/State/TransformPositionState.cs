using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves only the position of a Transform.
    /// </summary>
    public class TransformPositionState : SaveState<Transform>
    {
        private Vector3 position;

        protected override void OnCapture(Transform target)
        {
            position = target.position;
        }

        protected override void OnRestore(Transform target)
        {
            target.position = position;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with any other Transform state that modifies position
            return otherState is TransformPositionRotationState ||
                   otherState is TransformFullState ||
                   otherState is TransformLocalState;
        }
    }
}