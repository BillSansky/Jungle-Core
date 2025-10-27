using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IBoundsValue : IValue<Bounds>
    {
    }

    [Serializable]
    public class BoundsValue : LocalValue<Bounds>, IBoundsValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class BoundsMethodInvokerValue : MethodInvokerValue<Bounds>, IBoundsValue
    {
    }
}
