using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IQuaternionValue contract.
    /// </summary>
    public interface IQuaternionValue : IValue<Quaternion>
    {
    }
    /// <summary>
    /// Stores a Quaternion value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class QuaternionValue : LocalValue<Quaternion>, IQuaternionValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Quaternion value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class QuaternionClassMembersValue : ClassMembersValue<Quaternion>, IQuaternionValue
    {
    }
}
