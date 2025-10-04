using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IColliderValue : IValue<Collider>
    {
    }

    [Serializable]
    public class ColliderLocalValue : LocalValue<Collider>, IColliderValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
