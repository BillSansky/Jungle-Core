using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IGameObjectValue : IGameObjectReference, IValue<GameObject>, IComponentReference
    {
        Component IComponentReference.Component => V.transform;

        GameObject G => Value();

        IEnumerable<GameObject> Gs => Values;
        
        GameObject IGameObjectReference.GameObject => Value();
    }

    [Serializable]
    [JungleClassInfo("GameObject Value", "Stores a GameObject reference directly on the owner.", null, "Values/Game Dev", true)]
    public class GameObjectValue : LocalValue<GameObject>, IGameObjectValue
    {
        public override bool HasMultipleValues => false;
        
        public static implicit operator GameObject(GameObjectValue value)
        {
            return value.Value();
        }
    }

    [Serializable]
    [JungleClassInfo("GameObject Member Value", "Returns a GameObject reference from a component field, property, or method.", null, "Values/Game Dev")]
    public class GameObjectClassMembersValue : ClassMembersValue<GameObject>, IGameObjectValue
    {
        public static implicit operator GameObject(GameObjectClassMembersValue value)
        {
            return value.Value();
        }
    }
}
