using System;
using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves Rigidbody2D body type (kinematic, dynamic, static).
    /// </summary>
    [Serializable]
    public class Rigidbody2DBodyTypeState : SaveState<Rigidbody2D>
    {
        private RigidbodyType2D bodyType;

        protected override void OnCapture(Rigidbody2D target)
        {
            bodyType = target.bodyType;
        }

        protected override void OnRestore(Rigidbody2D target)
        {
            target.bodyType = bodyType;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with full state that also modifies bodyType
            return otherState is Rigidbody2DFullState;
        }
    }
}