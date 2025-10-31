using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Defines the IStringValue contract.
    /// </summary>
    public interface IStringValue : IValue<string>
    {
    }
    /// <summary>
    /// Stores a string value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class StringValue : LocalValue<string>, IStringValue
    {
        public StringValue()
        {
        }

        public StringValue(string value)
            : base(value)
        {
        }

        public override bool HasMultipleValues => false;

    }
    /// <summary>
    /// Resolves a string value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class StringClassMembersValue : ClassMembersValue<string>, IStringValue
    {
    }
}
