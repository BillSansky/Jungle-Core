using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the IGameObjectValue contract.
    /// </summary>
    public interface IGameObjectValue : IGameObjectReference, IValue<GameObject>,IComponentReference
    {
        Component IComponentReference.Component => V.transform;

        GameObject G => Value();

        IEnumerable<GameObject> Gs => Values;
        
        GameObject IGameObjectReference.GameObject => Value();
    }
    /// <summary>
    /// Stores a GameObject reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class GameObjectValue : LocalValue<GameObject>, IGameObjectValue
    {
        public override bool HasMultipleValues => false;
        /// <summary>
        /// Implicitly converts the wrapper to the referenced GameObject.
        /// </summary>
        public static implicit operator GameObject(GameObjectValue value)
        {
            return value.Value();
        }
    }
    /// <summary>
    /// Resolves a GameObject reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class GameObjectClassMembersValue : ClassMembersValue<GameObject>, IGameObjectValue
    {
        /// <summary>
        /// Implicitly converts the reflected member value to a GameObject.
        /// </summary>
        public static implicit operator GameObject(GameObjectClassMembersValue value)
        {
            return value.Value();
        }
    }
}
