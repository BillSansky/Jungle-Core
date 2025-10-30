using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IGameObjectValue : IGameObjectReference, IValue<GameObject>
    {
        GameObject G => ((IValue<GameObject>)this).Value();

        IEnumerable<GameObject> Gs => ((IValue<GameObject>)this).Values;
        
        GameObject IGameObjectReference.GameObject => ((IValue<GameObject>)this).Value();
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
