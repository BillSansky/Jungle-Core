using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Defines the ILongValue contract.
    /// </summary>
    public interface ILongValue : IValue<long>
    {
    }
    /// <summary>
    /// Stores a long value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class LongValue : LocalValue<long>, ILongValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a long value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class LongClassMembersValue : ClassMembersValue<long>, ILongValue
    {
    }
}
