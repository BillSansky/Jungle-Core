using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface IRigidbodyValue : IValue<Rigidbody>
    {
    }

    [Serializable]
    public class RigidbodyValue : LocalValue<Rigidbody>, IRigidbodyValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class RigidbodyMethodInvokerValue : MethodInvokerValue<Rigidbody>, IRigidbodyValue
    {
    }
}
