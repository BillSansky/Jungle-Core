using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Bounds struct.
    /// </summary>
    public interface IBoundsValue : IValue<Bounds>
    {
    }
    public interface ISettableBoundsValue : IBoundsValue, IValueSableValue<Bounds>
    {
    }
    /// <summary>
    /// Stores a Bounds value locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bounds Value", "Stores a Bounds value locally on the owner.", null, "Values/Unity Primitives", true)]
    public class BoundsValue : LocalValue<Bounds>, ISettableBoundsValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a Bounds value from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bounds Member Value", "Returns a Bounds value from a component field, property, or method.", null, "Values/Unity Primitives")]
    public class BoundsClassMembersValue : ClassMembersValue<Bounds>, IBoundsValue
    {
    }
}
