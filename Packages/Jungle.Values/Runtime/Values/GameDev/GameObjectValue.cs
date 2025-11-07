using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Represents a value provider that returns a GameObject reference.
    /// </summary>
    public interface IGameObjectValue : IGameObjectReference, IValue<GameObject>, IComponentReference
    {
        Component IComponentReference.Component => V.transform;

        GameObject G => Value();

        IEnumerable<GameObject> Gs => Values;
        
        GameObject IGameObjectReference.GameObject => Value();
    }
    /// <summary>
    /// Stores a GameObject reference directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("GameObject Value", "Stores a GameObject reference directly on the owner.", null, "Values/Game Dev", true)]
    public class GameObjectValue : LocalValue<GameObject>, IGameObjectValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        /// <summary>
        /// Implicitly converts the value container to its stored GameObject.
        /// </summary>
        
        public static implicit operator GameObject(GameObjectValue value)
        {
            return value.Value();
        }
    }
    /// <summary>
    /// Returns a GameObject reference from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("GameObject Member Value", "Returns a GameObject reference from a component field, property, or method.", null, "Values/Game Dev")]
    public class GameObjectClassMembersValue : ClassMembersValue<GameObject>, IGameObjectValue
    {
        /// <summary>
        /// Implicitly converts the member-backed value to its GameObject.
        /// </summary>
        public static implicit operator GameObject(GameObjectClassMembersValue value)
        {
            return value.Value();
        }
    }
}
