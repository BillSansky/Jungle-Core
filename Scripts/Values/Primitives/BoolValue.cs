using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IBoolValue : IValue<bool>
    {
    }

    [Serializable]
    [JungleClassInfo("Bool Value", "Stores a boolean value directly on the owner.", null, "Values/Primitives", true)]
    public class BoolValue : LocalValue<bool>, IBoolValue
    {
        public BoolValue()
        {
        }

        public BoolValue(bool value)
            : base(value)
        {
        }

        public override bool HasMultipleValues => false;

    }

    [Serializable]
    [JungleClassInfo("Bool Member Value", "Returns a boolean from a component field, property, or method.", null, "Values/Primitives")] 
    public class BoolClassMembersValue : ClassMembersValue<bool>, IBoolValue
    {
    }
}
