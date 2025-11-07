using System;
using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves Rigidbody2D velocity and angular velocity.
    /// </summary>
    [Serializable]
    public class Rigidbody2DVelocityState : SaveState<Rigidbody2D>
    {
        private Vector2 velocity;
        private float angularVelocity;

        protected override void OnCapture(Rigidbody2D target)
        {
            velocity = target.velocity;
            angularVelocity = target.angularVelocity;
        }

        protected override void OnRestore(Rigidbody2D target)
        {
            target.velocity = velocity;
            target.angularVelocity = angularVelocity;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with full state that also modifies velocity
            return otherState is Rigidbody2DFullState;
        }
    }
}