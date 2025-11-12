using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Vector2 value.
    /// </summary>
    public interface IVector2Value : IValue<Vector2>
    {
    }
    public interface ISettableVector2Value : IVector2Value, IValueSableValue<Vector2>
    {
    }
    /// <summary>
    /// Stores a 2D vector locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2 Value", "Stores a 2D vector locally on the owner.", null, "Values/Unity Primitives", true)]
    public class Vector2Value : LocalValue<Vector2>, ISettableVector2Value
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a 2D vector from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2 Member Value", "Returns a 2D vector from a component field, property, or method.", null, "Values/Unity Primitives")]
    public class Vector2ClassMembersValue : ClassMembersValue<Vector2>, IVector2Value
    {
    }
}
