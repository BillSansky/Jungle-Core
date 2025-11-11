using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a 2D integer vector.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2Int value", fileName = "Vector2IntValue")]
    [JungleClassInfo("Vector2Int Value Asset", "ScriptableObject storing a 2D integer vector.", null, "Values/Unity Types")]
    public class Vector2IntValueAsset : ValueAsset<Vector2Int>
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
    /// ScriptableObject storing a list of 2D integer vectors.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2Int list value", fileName = "Vector2IntListValue")]
    [JungleClassInfo("Vector2Int List Asset", "ScriptableObject storing a list of 2D integer vectors.", null, "Values/Unity Types")]
    public class Vector2IntListValueAsset : SerializedValueListAsset<Vector2Int>
    {
    }
    /// <summary>
    /// Reads a 2D integer vector from a Vector2IntValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2Int Value From Asset", "Reads a 2D integer vector from a Vector2IntValueAsset.", null, "Values/Unity Types")]
    public class Vector2IntValueFromAsset :
        ValueFromAsset<Vector2Int, Vector2IntValueAsset>, IVector2IntValue
    {
    }
    /// <summary>
    /// Reads 2D integer vectors from a Vector2IntListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector2Int List From Asset", "Reads 2D integer vectors from a Vector2IntListValueAsset.", null, "Values/Unity Types")]
    public class Vector2IntListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector2Int>, Vector2IntListValueAsset>
    {
    }
}
