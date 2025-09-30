using System;

namespace Jungle.Values.Primitives
{
    public interface IDoubleValue : IValue<double>
    {
    }

    [Serializable]
    public class DoubleValue : LocalValue<double>, IDoubleValue
    {
    }
}
