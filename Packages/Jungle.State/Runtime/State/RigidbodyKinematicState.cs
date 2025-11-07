using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves Rigidbody kinematic state only.
    /// </summary>
    public class RigidbodyKinematicState : SaveState<Rigidbody>
    {
        private bool isKinematic;

        protected override void OnCapture(Rigidbody target)
        {
            isKinematic = target.isKinematic;
        }

        protected override void OnRestore(Rigidbody target)
        {
            target.isKinematic = isKinematic;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with full state that also modifies isKinematic
            return otherState is RigidbodyFullState;
        }
    }
}