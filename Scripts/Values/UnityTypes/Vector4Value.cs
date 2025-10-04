using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector4Value : IValue<Vector4>
    {
    }

    [Serializable]
    public class Vector4Value : LocalValue<Vector4>, IVector4Value
    {
        public override bool HasMultipleValues => false;
        
    }
}
