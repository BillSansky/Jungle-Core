using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector3IntValue : IValue<Vector3Int>
    {
    }

    [Serializable]
    public class Vector3IntValue : LocalValue<Vector3Int>, IVector3IntValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class Vector3IntMethodInvokerValue : MethodInvokerValue<Vector3Int>, IVector3IntValue
    {
    }
}
