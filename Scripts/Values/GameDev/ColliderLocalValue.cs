using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IColliderValue : IComponent<Collider>
    {
    }

    [Serializable]
    [JungleClassInfo("Collider Value", "Stores a collider component directly on the owner.", null, "Values/Game Dev", true)]
    public class ColliderLocalValue : LocalValue<Collider>, IColliderValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Collider Member Value", "Returns a collider component from a component field, property, or method.", null, "Values/Game Dev")]
    public class ColliderClassMembersValue : ClassMembersValue<Collider>, IColliderValue
    {
    }
}
