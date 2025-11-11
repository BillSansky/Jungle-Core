using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Represents a value provider that returns a floating-point value.
    /// </summary>
    public interface IFloatValue : IValue<float>
    {
    }
    /// <summary>
    /// Stores a floating-point number directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Float Value", "Stores a floating-point number directly on the owner.", null, "Values/Primitives", true)]
    public class FloatValue : LocalValue<float>, IFloatValue
    {
        /// <summary>
        /// Initializes a new instance of the FloatValue.
        /// </summary>
        public FloatValue()
        {
        }
        /// <summary>
        /// Initializes a new instance of the FloatValue.
        /// </summary>

        public FloatValue(float value)
            : base(value)
        {
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public override bool HasMultipleValues => false;

    }
    /// <summary>
    /// Returns a floating-point number from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Float Member Value", "Returns a floating-point number from a component field, property, or method.", null, "Values/Primitives")]
    public class FloatClassMembersValue : ClassMembersValue<float>, IFloatValue
    {
    }
}
