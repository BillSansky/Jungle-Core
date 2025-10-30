using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface ILongValue : IValue<long>
    {
    }

    [Serializable]
    public class LongValue : LocalValue<long>, ILongValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class LongClassMembersValue : ClassMembersValue<long>, ILongValue
    {
    }
}
