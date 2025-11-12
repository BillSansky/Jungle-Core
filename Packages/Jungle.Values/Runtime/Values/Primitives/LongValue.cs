using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Represents a value provider that returns a long integer value.
    /// </summary>
    public interface ILongValue : IValue<long>
    {
    }
    public interface ISettableLongValue : ILongValue, IValueSableValue<long>
    {
    }
    /// <summary>
    /// Stores a 64-bit integer directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Long Value", "Stores a 64-bit integer directly on the owner.", null, "Primitives", true)]
    public class LongValue : LocalValue<long>, ISettableLongValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a 64-bit integer from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Long Member Value", "Returns a 64-bit integer from a component field, property, or method.", null, "Primitives")]
    public class LongClassMembersValue : ClassMembersValue<long>, ILongValue
    {
    }
}
