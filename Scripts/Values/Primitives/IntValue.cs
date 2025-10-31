using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Defines the IIntValue contract.
    /// </summary>
    public interface IIntValue : IValue<int>
    {
    }
    /// <summary>
    /// Stores an int value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class IntValue : LocalValue<int>, IIntValue
    {
        /// <summary>
        /// Initializes the value with the provided integer.
        /// </summary>
        public IntValue(int i) : base(i)
        {
           
        }

        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves an int value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class IntClassMembersValue : ClassMembersValue<int>, IIntValue
    {
    }
}
