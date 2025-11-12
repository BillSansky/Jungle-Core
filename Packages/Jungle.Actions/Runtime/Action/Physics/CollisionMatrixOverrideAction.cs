using System;
using Jungle.Attributes;
using Jungle.Physics;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [Serializable]
    [JungleClassInfo("Collision Matrix Override", "Overrides the global physics collision matrix.", null, "Actions/Physics")]
    public class CollisionMatrixOverrideAction : IImmediateAction
    {
        [SerializeReference]
        private ICollisionMatrixValue collisionMatrix = new CollisionMatrixValue();

        [SerializeField]
        private bool restoreOnStop = true;

        private CollisionLayerMatrix cachedMatrix;
        private bool hasCachedMatrix;

        public void StartProcess(Action callback = null)
        {
            if (restoreOnStop && !hasCachedMatrix)
            {
                cachedMatrix = CollisionLayerMatrix.FromPhysicsSettings();
                hasCachedMatrix = true;
            }

            CollisionLayerMatrix matrix = collisionMatrix.Value();
            matrix.ApplyToPhysics();
            callback?.Invoke();
        }

        public void Stop()
        {
            if (restoreOnStop && hasCachedMatrix)
            {
                cachedMatrix.ApplyToPhysics();
                hasCachedMatrix = false;
            }
        }
    }
}
