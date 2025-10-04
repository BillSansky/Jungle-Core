using System;

namespace Jungle.Values.Primitives
{
    public interface IBoolValue : IValue<bool>
    {
    }

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

}
