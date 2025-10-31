using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IBoundsValue contract.
    /// </summary>
    public interface IBoundsValue : IValue<Bounds>
    {
    }
    /// <summary>
    /// Stores a Bounds value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class BoundsValue : LocalValue<Bounds>, IBoundsValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Bounds value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class BoundsClassMembersValue : ClassMembersValue<Bounds>, IBoundsValue
    {
    }
}
