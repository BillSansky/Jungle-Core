using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IStringValue : IValue<string>
    {
    }

    [Serializable]
    [JungleClassInfo("String Value", "Stores a text string directly on the owner.", null, "Values/Primitives", true)]
    public class StringValue : LocalValue<string>, IStringValue
    {
        public StringValue()
        {
        }

        public StringValue(string value)
            : base(value)
        {
        }

        public override bool HasMultipleValues => false;

    }

    [Serializable]
    [JungleClassInfo("String Member Value", "Returns a text string from a component field, property, or method.", null, "Values/Primitives")]
    public class StringClassMembersValue : ClassMembersValue<string>, IStringValue
    {
    }
}
