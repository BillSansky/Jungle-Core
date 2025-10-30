using System;
using Jungle.Values;
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

    [Serializable]
    public class Vector4ClassMembersValue : ClassMembersValue<Vector4>, IVector4Value
    {
    }
}
