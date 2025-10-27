using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IStringValue : IValue<string>
    {
    }

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

    [Serializable]
    public class StringMethodInvokerValue : MethodInvokerValue<string>, IStringValue
    {
    }
}
