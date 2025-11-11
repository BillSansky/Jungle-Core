using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves Rigidbody velocity and angular velocity.
    /// </summary>
    public class RigidbodyVelocityState : SaveState<Rigidbody>
    {
        private Vector3 velocity;
        private Vector3 angularVelocity;

        protected override void OnCapture(Rigidbody target)
        {
            velocity = target.linearVelocity;
            angularVelocity = target.angularVelocity;
        }

        protected override void OnRestore(Rigidbody target)
        {
            target.linearVelocity = velocity;
            target.angularVelocity = angularVelocity;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with full state that also modifies velocity
            return otherState is RigidbodyFullState;
        }
    }
}