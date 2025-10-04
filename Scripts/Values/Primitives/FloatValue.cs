using System;

namespace Jungle.Values.Primitives
{
    public interface IFloatValue : IValue<float>
    {
    }

    [Serializable]
    public class FloatValue : LocalValue<float>, IFloatValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
