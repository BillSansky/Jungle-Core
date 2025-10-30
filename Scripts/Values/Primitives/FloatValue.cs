using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IFloatValue : IValue<float>
    {
    }

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

    [Serializable]
    public class FloatClassMembersValue : ClassMembersValue<float>, IFloatValue
    {
    }
}
