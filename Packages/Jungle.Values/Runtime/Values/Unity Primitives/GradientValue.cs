using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Gradient value.
    /// </summary>
    public interface IGradientValue : IValue<Gradient>
    {
    }
    public interface ISettableGradientValue : IGradientValue, IValueSableValue<Gradient>
    {
    }
    /// <summary>
    /// Stores a gradient locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Gradient Value", "Stores a gradient locally on the owner.", null, "Unity Types", true)]
    public class GradientValue : LocalValue<Gradient>, ISettableGradientValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a gradient from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Gradient Member Value", "Returns a gradient from a component field, property, or method.", null, "Unity Types")]
    public class GradientClassMembersValue : ClassMembersValue<Gradient>, IGradientValue
    {
    }
}
