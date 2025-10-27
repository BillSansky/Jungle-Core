using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IDoubleValue : IValue<double>
    {
    }

    [Serializable]
    public class DoubleValue : LocalValue<double>, IDoubleValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class DoubleMethodInvokerValue : MethodInvokerValue<double>, IDoubleValue
    {
    }
}
