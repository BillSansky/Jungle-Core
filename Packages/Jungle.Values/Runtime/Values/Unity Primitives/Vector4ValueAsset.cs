using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a 4D vector.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector4 value", fileName = "Vector4Value")]
    [JungleClassInfo("Vector4 Value Asset", "ScriptableObject storing a 4D vector.", null, "Values/Unity Primitives")]
    public class Vector4ValueAsset : ValueAsset<Vector4>
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
    /// ScriptableObject storing a list of 4D vectors.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector4 list value", fileName = "Vector4ListValue")]
    [JungleClassInfo("Vector4 List Asset", "ScriptableObject storing a list of 4D vectors.", null, "Values/Unity Primitives")]
    public class Vector4ListValueAsset : SerializedValueListAsset<Vector4>
    {
    }
    /// <summary>
    /// Reads a 4D vector from a Vector4ValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector4 Value From Asset", "Reads a 4D vector from a Vector4ValueAsset.", null, "Values/Unity Primitives")]
    public class Vector4ValueFromAsset : ValueFromAsset<Vector4, Vector4ValueAsset>, ISettableVector4Value
    {
    }
    /// <summary>
    /// Reads 4D vectors from a Vector4ListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector4 List From Asset", "Reads 4D vectors from a Vector4ListValueAsset.", null, "Values/Unity Primitives")]
    public class Vector4ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector4>, Vector4ListValueAsset>
    {
    }
}
