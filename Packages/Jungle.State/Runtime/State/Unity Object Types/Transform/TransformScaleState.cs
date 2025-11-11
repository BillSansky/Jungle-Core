using UnityEngine;

namespace Jungle.Utility
{
    /// <summary>
    /// Saves only the scale of a Transform.
    /// </summary>
    public class TransformScaleState : SaveState<Transform>
    {
        private Vector3 localScale;

        protected override void OnCapture(Transform target)
        {
            localScale = target.localScale;
        }

        protected override void OnRestore(Transform target)
        {
            target.localScale = localScale;
        }

        protected override bool OnCheckConflict(ISaveState otherState)
        {
            // Conflicts with states that modify scale
            return otherState is TransformFullState ||
                   otherState is TransformLocalState;
        }
    }
}