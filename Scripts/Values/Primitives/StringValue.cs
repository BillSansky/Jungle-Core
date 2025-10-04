using System;

namespace Jungle.Values.Primitives
{
    public interface IStringValue : IValue<string>
    {
    }

    [Serializable]
    public class StringValue : LocalValue<string>, IStringValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
