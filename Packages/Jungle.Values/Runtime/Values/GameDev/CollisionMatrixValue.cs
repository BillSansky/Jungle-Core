using System;
using Jungle.Attributes;
using Jungle.Physics;
using Jungle.Values;

namespace Jungle.Values.GameDev
{
    public interface ICollisionMatrixValue : IValue<CollisionLayerMatrix>
    {
    }

    [Serializable]
    [JungleClassInfo("Collision Matrix Value", "Stores a physics collision matrix directly on the owner.", null, "Game Dev", true)]
    public class CollisionMatrixValue : LocalValue<CollisionLayerMatrix>, ICollisionMatrixValue
    {
        public CollisionMatrixValue()
            : base(CollisionLayerMatrix.FromPhysicsSettings())
        {
        }

        public CollisionMatrixValue(CollisionLayerMatrix value)
            : base(value)
        {
        }

        public override bool HasMultipleValues => false;
    }
}
