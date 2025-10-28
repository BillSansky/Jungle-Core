using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IGameObjectValue : IValue<GameObject>, ITransformValue
    {
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
        
    }

    [Serializable]
    public class GameObjectMethodInvokerValue : MethodInvokerValue<GameObject>, IGameObjectValue
    {
    }
}
