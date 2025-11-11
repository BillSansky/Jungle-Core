using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a 2D integer vector.
    /// </summary>
    [JungleClassInfo("Vector2Int Value Component", "Component exposing a 2D integer vector.", null, "Values/Unity Types")]
    public class Vector2IntValueComponent : ValueComponent<Vector2Int>
    {
        [SerializeField]
        private Vector2Int value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Vector2Int Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Vector2Int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of 2D integer vectors.
    /// </summary>

    [JungleClassInfo("Vector2Int List Component", "Component exposing a list of 2D integer vectors.", null, "Values/Unity Types")]
    public class Vector2IntListValueComponent : SerializedValueListComponent<Vector2Int>
    {
    }
    /// <summary>
    /// Reads a 2D integer vector from a Vector2IntValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2Int Value From Component", "Reads a 2D integer vector from a Vector2IntValueComponent.", null, "Values/Unity Types")]
    public class Vector2IntValueFromComponent :
        ValueFromComponent<Vector2Int, Vector2IntValueComponent>, ISettableVector2IntValue
    {
    }
    /// <summary>
    /// Reads 2D integer vectors from a Vector2IntListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2Int List From Component", "Reads 2D integer vectors from a Vector2IntListValueComponent.", null, "Values/Unity Types")]
    public class Vector2IntListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector2Int>, Vector2IntListValueComponent>
    {
    }
}
