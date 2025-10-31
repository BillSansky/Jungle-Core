using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Defines the IFloatValue contract.
    /// </summary>
    public interface IFloatValue : IValue<float>
    {
    }
    /// <summary>
    /// Stores a float value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class FloatValue : LocalValue<float>, IFloatValue
    {
        public FloatValue()
        {
        }

        public FloatValue(float value)
            : base(value)
        {
        }

        public override bool HasMultipleValues => false;

    }
    /// <summary>
    /// Resolves a float value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class FloatClassMembersValue : ClassMembersValue<float>, IFloatValue
    {
    }
}
