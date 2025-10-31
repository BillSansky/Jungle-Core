using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Defines the IBoolValue contract.
    /// </summary>
    public interface IBoolValue : IValue<bool>
    {
    }
    /// <summary>
    /// Stores a boolean value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class BoolValue : LocalValue<bool>, IBoolValue
    {
        public BoolValue()
        {
        }

        public BoolValue(bool value)
            : base(value)
        {
        }

        public override bool HasMultipleValues => false;

    }
    /// <summary>
    /// Resolves a boolean value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class BoolClassMembersValue : ClassMembersValue<bool>, IBoolValue
    {
    }
}
