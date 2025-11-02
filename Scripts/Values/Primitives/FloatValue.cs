using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IFloatValue : IValue<float>
    {
    }

    [Serializable]
    [JungleClassInfo("Float Value", "Stores a floating-point number directly on the owner.", null, "Values/Primitives", true)]
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
    [JungleClassInfo("Float Member Value", "Returns a floating-point number from a component field, property, or method.", null, "Values/Primitives")]
    public class FloatClassMembersValue : ClassMembersValue<float>, IFloatValue
    {
    }
}
