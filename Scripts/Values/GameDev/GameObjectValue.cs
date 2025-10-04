using System;
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
}
