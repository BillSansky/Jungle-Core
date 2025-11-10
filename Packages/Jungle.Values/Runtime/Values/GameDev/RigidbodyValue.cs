using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Provides access to a Rigidbody reference.
    /// </summary>
    public interface IRigidbodyValue : IComponent<Rigidbody>
    {
    }
    /// <summary>
    /// Stores a rigidbody component directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rigidbody Value", "Stores a rigidbody component directly on the owner.", null, "Values/Game Dev", true)]
    public class RigidbodyValue : LocalValue<Rigidbody>, IRigidbodyValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a rigidbody component from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rigidbody Member Value", "Returns a rigidbody component from a component field, property, or method.", null, "Values/Game Dev")]
    public class RigidbodyClassMembersValue : ComponentClassMembersValue<Rigidbody>, IRigidbodyValue
    {
    }
}
