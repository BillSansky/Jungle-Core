using System;

namespace Jungle.Values.Primitives
{
    public interface IIntValue : IValue<int>
    {
    }

    [Serializable]
    public class IntValue : LocalValue<int>, IIntValue
    {
    }
}
