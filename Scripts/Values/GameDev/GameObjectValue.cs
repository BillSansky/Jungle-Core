using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IGameObjectValue : IValue<GameObject>, ITransformValue
    {
        GameObject G => ((IValue<GameObject>)this).Value();

        IEnumerable<GameObject> Gs => ((IValue<GameObject>)this).Values;
        
        Transform IValue<Transform>.Value()
        {
            return ((IValue<GameObject>)this).Value().transform;
        }

        IEnumerable<Transform> IValue<Transform>.Values
        {
            get
            {
                foreach (var gameObject in ((IValue<GameObject>)this).Values)
                {
                    yield return gameObject.transform;
                }
            }
        }
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
    public class GameObjectMethodInvokerValue : MethodInvokerValue<GameObject>, IGameObjectValue
    {
        public static implicit operator GameObject(GameObjectMethodInvokerValue value)
        {
            return value.Value();
        }
    }
}
