using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Represents a value provider that returns a double-precision floating-point value.
    /// </summary>
    public interface IDoubleValue : IValue<double>
    {
    }
    /// <summary>
    /// Stores a double-precision number directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Double Value", "Stores a double-precision number directly on the owner.", null, "Values/Primitives", true)]
    public class DoubleValue : LocalValue<double>, IDoubleValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a double-precision number from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Double Member Value", "Returns a double-precision number from a component field, property, or method.", null, "Values/Primitives")]
    public class DoubleClassMembersValue : ClassMembersValue<double>, IDoubleValue
    {
    }
}
