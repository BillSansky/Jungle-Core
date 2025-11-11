using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Represents a value provider that returns a boolean value.
    /// </summary>
    public interface IBoolValue : IValue<bool>
    {
    }
    public interface ISettableBoolValue : IBoolValue, IValueSableValue<bool>
    {
    }
    /// <summary>
    /// Stores a boolean value directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bool Value", "Stores a boolean value directly on the owner.", null, "Values/Primitives", true)]
    public class BoolValue : LocalValue<bool>, ISettableBoolValue
    {
        /// <summary>
        /// Initializes a new instance of the BoolValue.
        /// </summary>
        public BoolValue()
        {
        }
        /// <summary>
        /// Initializes a new instance of the BoolValue.
        /// </summary>

        public BoolValue(bool value)
            : base(value)
        {
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public override bool HasMultipleValues => false;

    }
    /// <summary>
    /// Returns a boolean from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bool Member Value", "Returns a boolean from a component field, property, or method.", null, "Values/Primitives")] 
    public class BoolClassMembersValue : ClassMembersValue<bool>, IBoolValue
    {
    }
}
