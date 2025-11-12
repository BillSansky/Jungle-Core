using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a 4D vector.
    /// </summary>
    [JungleClassInfo("Vector4 Value Component", "Component exposing a 4D vector.", null, "Values/Unity Primitives")]
    public class Vector4ValueComponent : ValueComponent<Vector4>
    {
        [SerializeField]
        private Vector4 value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Vector4 Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Vector4 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of 4D vectors.
    /// </summary>

    [JungleClassInfo("Vector4 List Component", "Component exposing a list of 4D vectors.", null, "Values/Unity Primitives")]
    public class Vector4ListValueComponent : SerializedValueListComponent<Vector4>
    {
    }
    /// <summary>
    /// Reads a 4D vector from a Vector4ValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector4 Value From Component", "Reads a 4D vector from a Vector4ValueComponent.", null, "Values/Unity Primitives")]
    public class Vector4ValueFromComponent : ValueFromComponent<Vector4, Vector4ValueComponent>, ISettableVector4Value
    {
    }
    /// <summary>
    /// Reads 4D vectors from a Vector4ListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector4 List From Component", "Reads 4D vectors from a Vector4ListValueComponent.", null, "Values/Unity Primitives")]
    public class Vector4ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector4>, Vector4ListValueComponent>
    {
    }
}
