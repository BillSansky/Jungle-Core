using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves full Rigidbody state including physics properties.
    /// </summary>
    public class RigidbodyFullState : SaveState<Rigidbody>
    {
        private Vector3 velocity;
        private Vector3 angularVelocity;
        private bool isKinematic;
        private bool useGravity;
        private float mass;
        private float drag;
        private float angularDrag;

        protected override void OnCapture(Rigidbody target)
        {
            velocity = target.linearVelocity;
            angularVelocity = target.angularVelocity;
            isKinematic = target.isKinematic;
            useGravity = target.useGravity;
            mass = target.mass;
            drag = target.linearDamping;
            angularDrag = target.angularDamping;
        }

        protected override void OnRestore(Rigidbody target)
        {
            target.isKinematic = isKinematic;
            target.useGravity = useGravity;
            target.mass = mass;
            target.linearDamping = drag;
            target.angularDamping = angularDrag;
            target.linearVelocity = velocity;
            target.angularVelocity = angularVelocity;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with any other Rigidbody state
            return otherState is RigidbodyKinematicState ||
                   otherState is RigidbodyVelocityState;
        }
    }
}