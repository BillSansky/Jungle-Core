using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Color value.
    /// </summary>
    public interface IColorValue : IValue<Color>
    {
    }
    public interface ISettableColorValue : IColorValue, IValueSableValue<Color>
    {
    }
    /// <summary>
    /// Stores a Color value locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color Value", "Stores a Color value locally on the owner.", null, "Values/Unity Primitives", true)]
    public class ColorValue : LocalValue<Color>, ISettableColorValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a Color value from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color Member Value", "Returns a Color value from a component field, property, or method.", null, "Values/Unity Primitives")]
    public class ColorClassMembersValue : ClassMembersValue<Color>, IColorValue
    {
    }
}
