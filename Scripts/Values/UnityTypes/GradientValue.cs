using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IGradientValue contract.
    /// </summary>
    public interface IGradientValue : IValue<Gradient>
    {
    }
    /// <summary>
    /// Stores a Gradient value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class GradientValue : LocalValue<Gradient>, IGradientValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Gradient value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class GradientClassMembersValue : ClassMembersValue<Gradient>, IGradientValue
    {
    }
}
