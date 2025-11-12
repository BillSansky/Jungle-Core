using System;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Provides access to a physics collision hit information.
    /// </summary>
    public interface ICollisionHitValue : IValue<RaycastHit>
    {
    }

    /// <summary>
    /// Stores raycast hit information directly on the owner.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit Value", "Stores raycast hit information directly on the owner.", null, "Game Dev", true)]
    public class CollisionHitLocalValue : LocalValue<RaycastHit>, ICollisionHitValue
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionHitLocalValue"/> class.
        /// </summary>
        public CollisionHitLocalValue()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollisionHitLocalValue"/> class with a starting value.
        /// </summary>
        /// <param name="value">Initial raycast hit value.</param>
        public CollisionHitLocalValue(RaycastHit value)
            : base(value)
        {
        }

        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
    }

    /// <summary>
    /// Returns raycast hit information from a component field, property, or method.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collision Hit Member Value", "Returns raycast hit information from a component field, property, or method.", null, "Game Dev")]
    public class CollisionHitClassMembersValue : ClassMembersValue<RaycastHit>, ICollisionHitValue
    {
    }
}
