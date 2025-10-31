using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Vector2 value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 value", fileName = "Vector2Value")]
    public class Vector2ValueAsset : ValueAsset<Vector2>
    {
        [SerializeField]
        private Vector2 value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Vector2 Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Vector2 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Vector2 values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 list value", fileName = "Vector2ListValue")]
    public class Vector2ListValueAsset : SerializedValueListAsset<Vector2>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector2 value from the assigned Vector2ValueAsset.
    /// </summary>
    [Serializable]
    public class Vector2ValueFromAsset : ValueFromAsset<Vector2, Vector2ValueAsset>, IVector2Value
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector2 values from an assigned Vector2ListValueAsset.
    /// </summary>
    [Serializable]
    public class Vector2ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector2>, Vector2ListValueAsset>
    {
    }
}
