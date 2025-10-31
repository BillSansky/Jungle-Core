using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the IRigidbodyValue contract.
    /// </summary>
    public interface IRigidbodyValue : IComponent<Rigidbody>
    {
    }
    /// <summary>
    /// Stores a Rigidbody reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class RigidbodyValue : LocalValue<Rigidbody>, IRigidbodyValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Rigidbody reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class RigidbodyClassMembersValue : ClassMembersValue<Rigidbody>, IRigidbodyValue
    {
    }
}
