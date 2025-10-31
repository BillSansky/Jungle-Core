using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Vector2Int value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2Int value", fileName = "Vector2IntValue")]
    public class Vector2IntValueAsset : ValueAsset<Vector2Int>
    {
        [SerializeField]
        private Vector2Int value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Vector2Int Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Vector2Int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Vector2Int values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2Int list value", fileName = "Vector2IntListValue")]
    public class Vector2IntListValueAsset : SerializedValueListAsset<Vector2Int>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector2Int value from an assigned Vector2IntValueAsset.
    /// </summary>
    [Serializable]
    public class Vector2IntValueFromAsset :
        ValueFromAsset<Vector2Int, Vector2IntValueAsset>, IVector2IntValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector2Int values from an assigned Vector2IntListValueAsset.
    /// </summary>
    [Serializable]
    public class Vector2IntListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector2Int>, Vector2IntListValueAsset>
    {
    }
}
