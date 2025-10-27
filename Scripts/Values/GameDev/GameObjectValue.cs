using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IGameObjectValue : IValue<GameObject>
    {
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
