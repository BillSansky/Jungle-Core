using System;
using Jungle.Values;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Defines the IDoubleValue contract.
    /// </summary>
    public interface IDoubleValue : IValue<double>
    {
    }
    /// <summary>
    /// Stores a double value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class DoubleValue : LocalValue<double>, IDoubleValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a double value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class DoubleClassMembersValue : ClassMembersValue<double>, IDoubleValue
    {
    }
}
