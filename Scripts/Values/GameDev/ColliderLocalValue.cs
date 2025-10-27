using System;
using Jungle.Values;
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

    [Serializable]
    public class ColliderMethodInvokerValue : MethodInvokerValue<Collider>, IColliderValue
    {
    }
}
