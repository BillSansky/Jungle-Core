using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a 2D vector.
    /// </summary>
    [JungleClassInfo("Vector2 Value Component", "Component exposing a 2D vector.", null, "Values/Unity Types")]
    public class Vector2ValueComponent : ValueComponent<Vector2>
    {
        [SerializeField]
        private Vector2 value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Vector2 Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Vector2 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of 2D vectors.
    /// </summary>

    [JungleClassInfo("Vector2 List Component", "Component exposing a list of 2D vectors.", null, "Values/Unity Types")]
    public class Vector2ListValueComponent : SerializedValueListComponent<Vector2>
    {
    }
    /// <summary>
    /// Reads a 2D vector from a Vector2ValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2 Value From Component", "Reads a 2D vector from a Vector2ValueComponent.", null, "Values/Unity Types")]
    public class Vector2ValueFromComponent : ValueFromComponent<Vector2, Vector2ValueComponent>, IVector2Value
    {
    }
    /// <summary>
    /// Reads 2D vectors from a Vector2ListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2 List From Component", "Reads 2D vectors from a Vector2ListValueComponent.", null, "Values/Unity Types")]
    public class Vector2ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector2>, Vector2ListValueComponent>
    {
    }
}
