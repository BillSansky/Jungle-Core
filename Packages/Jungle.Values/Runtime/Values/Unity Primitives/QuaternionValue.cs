using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Quaternion rotation.
    /// </summary>
    public interface IQuaternionValue : IValue<Quaternion>
    {
    }
    public interface ISettableQuaternionValue : IQuaternionValue, IValueSableValue<Quaternion>
    {
    }
    /// <summary>
    /// Stores a rotation quaternion locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Quaternion Value", "Stores a rotation quaternion locally on the owner.", null, "Unity Types", true)]
    public class QuaternionValue : LocalValue<Quaternion>, ISettableQuaternionValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a rotation quaternion from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Quaternion Member Value", "Returns a rotation quaternion from a component field, property, or method.", null, "Unity Types")]
    public class QuaternionClassMembersValue : ClassMembersValue<Quaternion>, IQuaternionValue
    {
    }
}
