using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface ILongValue : IValue<long>
    {
    }

    [Serializable]
    [JungleClassInfo("Long Value", "Stores a 64-bit integer directly on the owner.", null, "Values/Primitives", true)]
    public class LongValue : LocalValue<long>, ILongValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Long Member Value", "Returns a 64-bit integer from a component field, property, or method.", null, "Values/Primitives")]
    public class LongClassMembersValue : ClassMembersValue<long>, ILongValue
    {
    }
}
