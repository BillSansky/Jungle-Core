using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Represents a value provider that returns a GameObject reference.
    /// </summary>
    public interface IGameObjectValue : IValue<GameObject>
    {

    }
    public interface ISettableGameObjectValue : IGameObjectValue, IValueSableValue<GameObject>
    {
    }
    /// <summary>
    /// Stores a GameObject reference directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("GameObject Value", "Stores a GameObject reference directly on the owner.", null, "Game Dev", true)]
    public class GameObjectValue : LocalValue<GameObject>, ISettableGameObjectValue
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
    [JungleClassInfo("GameObject Member Value", "Returns a GameObject reference from a component field, property, or method.", null, "Game Dev")]
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
