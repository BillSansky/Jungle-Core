using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the IColliderValue contract.
    /// </summary>
    public interface IColliderValue : IComponent<Collider>
    {
    }
    /// <summary>
    /// Stores a Collider reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class ColliderLocalValue : LocalValue<Collider>, IColliderValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Collider reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class ColliderClassMembersValue : ClassMembersValue<Collider>, IColliderValue
    {
    }
}
