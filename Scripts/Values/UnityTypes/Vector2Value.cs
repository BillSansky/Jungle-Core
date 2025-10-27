using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector2Value : IValue<Vector2>
    {
    }

    [Serializable]
    public class Vector2Value : LocalValue<Vector2>, IVector2Value
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class Vector2MethodInvokerValue : MethodInvokerValue<Vector2>, IVector2Value
    {
    }
}
