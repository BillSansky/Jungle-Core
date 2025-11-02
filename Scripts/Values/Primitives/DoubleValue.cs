using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IDoubleValue : IValue<double>
    {
    }

    [Serializable]
    [JungleClassInfo("Double Value", "Stores a double-precision number directly on the owner.", null, "Values/Primitives", true)]
    public class DoubleValue : LocalValue<double>, IDoubleValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Double Member Value", "Returns a double-precision number from a component field, property, or method.", null, "Values/Primitives")]
    public class DoubleClassMembersValue : ClassMembersValue<double>, IDoubleValue
    {
    }
}
