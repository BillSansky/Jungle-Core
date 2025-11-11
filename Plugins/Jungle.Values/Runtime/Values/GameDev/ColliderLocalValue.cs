using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Provides access to a Collider reference.
    /// </summary>
    public interface IColliderValue : IValue<Collider>
    {
    }
    /// <summary>
    /// Stores a collider component directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Collider Value", "Stores a collider component directly on the owner.", null, "Values/Game Dev", true)]
    public class ColliderLocalValue : LocalValue<Collider>, IColliderValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a collider component from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Collider Member Value", "Returns a collider component from a component field, property, or method.", null, "Values/Game Dev")]
    public class ColliderClassMembersValue : ClassMembersValue<Collider>, IColliderValue
    {
    }
}
