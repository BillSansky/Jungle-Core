using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    public interface IIntValue : IValue<int>
    {
    }

    [Serializable]
    [JungleClassInfo("Int Value", "Stores a integer number directly on the owner.", null, "Values/Primitives", true)]
    public class IntValue : LocalValue<int>, IIntValue
    {
        public IntValue(int i) : base(i)
        {
           
        }

        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Int Member Value", "Returns a integer number from a component field, property, or method.", null, "Values/Primitives")]
    public class IntClassMembersValue : ClassMembersValue<int>, IIntValue
    {
    }
}
