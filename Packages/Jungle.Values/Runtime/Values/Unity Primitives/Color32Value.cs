using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Color32 value.
    /// </summary>
    public interface IColor32Value : IValue<Color32>
    {
    }
    public interface ISettableColor32Value : IColor32Value, IValueSableValue<Color32>
    {
    }
    /// <summary>
    /// Stores a Color32 value locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color32 Value", "Stores a Color32 value locally on the owner.", null, "Values/Unity Primitives", true)]
    public class Color32Value : LocalValue<Color32>, ISettableColor32Value
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a Color32 value from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color32 Member Value", "Returns a Color32 value from a component field, property, or method.", null, "Values/Unity Primitives")]
    public class Color32ClassMembersValue : ClassMembersValue<Color32>, IColor32Value
    {
    }
}
