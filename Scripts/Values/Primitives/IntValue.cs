using System;
using Jungle.Attributes;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Represents a value provider that returns an integer value.
    /// </summary>
    public interface IIntValue : IValue<int>
    {
    }
    /// <summary>
    /// Stores an integer value directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Int Value", "Stores an integer number directly on the owner.", null, "Values/Primitives", true)]
    public class IntValue : LocalValue<int>, IIntValue
    {
        /// <summary>
        /// Initializes a new instance of the IntValue.
        /// </summary>
        public IntValue(int i) : base(i)
        {
           
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns an integer value from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Int Member Value", "Returns an integer number from a component field, property, or method.", null, "Values/Primitives")]
    public class IntClassMembersValue : ClassMembersValue<int>, IIntValue
    {
    }
}
