using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IIntValue : IValue<int>
    {
    }

    [Serializable]
    public class IntValue : LocalValue<int>, IIntValue
    {
        public IntValue(int i) : base(i)
        {
           
        }

        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class IntMethodInvokerValue : MethodInvokerValue<int>, IIntValue
    {
    }
}
