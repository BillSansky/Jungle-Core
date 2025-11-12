using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Represents a value provider that returns a string value.
    /// </summary>
    public interface IStringValue : IValue<string>
    {
    }
    public interface ISettableStringValue : IStringValue, IValueSableValue<string>
    {
    }
    /// <summary>
    /// Stores a text string directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("String Value", "Stores a text string directly on the owner.", null, "Primitives", true)]
    public class StringValue : LocalValue<string>, ISettableStringValue
    {
        /// <summary>
        /// Initializes a new instance of the StringValue.
        /// </summary>
        public StringValue()
        {
        }
        /// <summary>
        /// Initializes a new instance of the StringValue.
        /// </summary>

        public StringValue(string value)
            : base(value)
        {
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public override bool HasMultipleValues => false;

    }
    /// <summary>
    /// Returns a text string from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("String Member Value", "Returns a text string from a component field, property, or method.", null, "Primitives")]
    public class StringClassMembersValue : ClassMembersValue<string>, IStringValue
    {
    }
}
