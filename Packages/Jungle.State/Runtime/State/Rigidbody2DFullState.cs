using System;
using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves full Rigidbody2D state.
    /// </summary>
    [Serializable]
    public class Rigidbody2DFullState : SaveState<Rigidbody2D>
    {
        private Vector2 velocity;
        private float angularVelocity;
        private RigidbodyType2D bodyType;
        private float gravityScale;
        private float mass;
        private float drag;
        private float angularDrag;

        protected override void OnCapture(Rigidbody2D target)
        {
            velocity = target.velocity;
            angularVelocity = target.angularVelocity;
            bodyType = target.bodyType;
            gravityScale = target.gravityScale;
            mass = target.mass;
            drag = target.drag;
            angularDrag = target.angularDrag;
        }

        protected override void OnRestore(Rigidbody2D target)
        {
            target.bodyType = bodyType;
            target.gravityScale = gravityScale;
            target.mass = mass;
            target.drag = drag;
            target.angularDrag = angularDrag;
            target.velocity = velocity;
            target.angularVelocity = angularVelocity;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with any other Rigidbody2D state
            return otherState is Rigidbody2DBodyTypeState ||
                   otherState is Rigidbody2DVelocityState;
        }
    }
}