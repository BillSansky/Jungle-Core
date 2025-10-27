using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IGradientValue : IValue<Gradient>
    {
    }

    [Serializable]
    public class GradientValue : LocalValue<Gradient>, IGradientValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class GradientMethodInvokerValue : MethodInvokerValue<Gradient>, IGradientValue
    {
    }
}
