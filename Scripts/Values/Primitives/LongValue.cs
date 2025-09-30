using System;

namespace Jungle.Values.Primitives
{
    public interface ILongValue : IValue<long>
    {
    }

    [Serializable]
    public class LongValue : LocalValue<long>, ILongValue
    {
    }
}
