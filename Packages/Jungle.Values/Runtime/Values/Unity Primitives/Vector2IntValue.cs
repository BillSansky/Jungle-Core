using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Vector2Int value.
    /// </summary>
    public interface IVector2IntValue : IValue<Vector2Int>
    {
    }
    public interface ISettableVector2IntValue : IVector2IntValue, IValueSableValue<Vector2Int>
    {
    }
    /// <summary>
    /// Stores a 2D integer vector locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2Int Value", "Stores a 2D integer vector locally on the owner.", null, "Values/Unity Types", true)]
    public class Vector2IntValue : LocalValue<Vector2Int>, ISettableVector2IntValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
        
    }
    /// <summary>
    /// Returns a 2D integer vector from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2Int Member Value", "Returns a 2D integer vector from a component field, property, or method.", null, "Values/Unity Types")]
    public class Vector2IntClassMembersValue : ClassMembersValue<Vector2Int>, IVector2IntValue
    {
    }
}
