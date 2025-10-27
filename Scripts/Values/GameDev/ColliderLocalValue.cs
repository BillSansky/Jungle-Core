using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IColliderValue : IComponent<Collider>
    {
    }

    [Serializable]
    public class ColliderLocalValue : LocalValue<Collider>, IColliderValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
