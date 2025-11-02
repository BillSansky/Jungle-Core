using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector4Value : IValue<Vector4>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector4 Value", "Stores a 4D vector locally on the owner.", null, "Values/Unity Types", true)]
    public class Vector4Value : LocalValue<Vector4>, IVector4Value
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Vector4 Member Value", "Returns a 4D vector from a component field, property, or method.", null, "Values/Unity Types")]
    public class Vector4ClassMembersValue : ClassMembersValue<Vector4>, IVector4Value
    {
    }
}
