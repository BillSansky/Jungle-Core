using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a 2D vector.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 value", fileName = "Vector2Value")]
    [JungleClassInfo("Vector2 Value Asset", "ScriptableObject storing a 2D vector.", null, "Values/Unity Types")]
    public class Vector2ValueAsset : ValueAsset<Vector2>
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
    /// ScriptableObject storing a list of 2D vectors.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 list value", fileName = "Vector2ListValue")]
    [JungleClassInfo("Vector2 List Asset", "ScriptableObject storing a list of 2D vectors.", null, "Values/Unity Types")]
    public class Vector2ListValueAsset : SerializedValueListAsset<Vector2>
    {
    }
    /// <summary>
    /// Reads a 2D vector from a Vector2ValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2 Value From Asset", "Reads a 2D vector from a Vector2ValueAsset.", null, "Values/Unity Types")]
    public class Vector2ValueFromAsset : ValueFromAsset<Vector2, Vector2ValueAsset>, ISettableVector2Value
    {
    }
    /// <summary>
    /// Reads 2D vectors from a Vector2ListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2 List From Asset", "Reads 2D vectors from a Vector2ListValueAsset.", null, "Values/Unity Types")]
    public class Vector2ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector2>, Vector2ListValueAsset>
    {
    }
}
