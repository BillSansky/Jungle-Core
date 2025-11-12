using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Vector4 value.
    /// </summary>
    public interface IVector4Value : IValue<Vector4>
    {
    }
    public interface ISettableVector4Value : IVector4Value, IValueSableValue<Vector4>
    {
    }
    /// <summary>
    /// Stores a 4D vector locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector4 Value", "Stores a 4D vector locally on the owner.", null, "Unity Types", true)]
    public class Vector4Value : LocalValue<Vector4>, ISettableVector4Value
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a 4D vector from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector4 Member Value", "Returns a 4D vector from a component field, property, or method.", null, "Unity Types")]
    public class Vector4ClassMembersValue : ClassMembersValue<Vector4>, IVector4Value
    {
    }
}
