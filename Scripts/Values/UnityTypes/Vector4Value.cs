using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IVector4Value contract.
    /// </summary>
    public interface IVector4Value : IValue<Vector4>
    {
    }
    /// <summary>
    /// Stores a Vector4 value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class Vector4Value : LocalValue<Vector4>, IVector4Value
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Vector4 value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class Vector4ClassMembersValue : ClassMembersValue<Vector4>, IVector4Value
    {
    }
}
