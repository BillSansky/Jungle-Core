using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IGameObjectValue : IGameObjectReference, IValue<GameObject>,IComponentReference
    {
        Component IComponentReference.Component => V.transform;

        GameObject G => Value();

        IEnumerable<GameObject> Gs => Values;
        
        GameObject IGameObjectReference.GameObject => Value();
    }

    [Serializable]
    public class GameObjectValue : LocalValue<GameObject>, IGameObjectValue
    {
        public override bool HasMultipleValues => false;
        
        public static implicit operator GameObject(GameObjectValue value)
        {
            return value.Value();
        }
    }

    [Serializable]
    public class GameObjectClassMembersValue : ClassMembersValue<GameObject>, IGameObjectValue
    {
        public static implicit operator GameObject(GameObjectClassMembersValue value)
        {
            return value.Value();
        }
    }
}
