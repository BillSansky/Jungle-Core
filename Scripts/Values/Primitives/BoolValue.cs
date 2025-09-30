using System;

namespace Jungle.Values.Primitives
{
    public interface IBoolValue : IValue<bool>
    {
    }

    [Serializable]
    public class BoolValue : LocalValue<bool>, IBoolValue
    {
    }

}
