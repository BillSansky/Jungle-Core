using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Vector4 value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector4 value", fileName = "Vector4Value")]
    public class Vector4ValueAsset : ValueAsset<Vector4>
    {
        [SerializeField]
        private Vector4 value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Vector4 Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Vector4 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Vector4 values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector4 list value", fileName = "Vector4ListValue")]
    public class Vector4ListValueAsset : SerializedValueListAsset<Vector4>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector4 value from the assigned Vector4ValueAsset.
    /// </summary>
    [Serializable]
    public class Vector4ValueFromAsset : ValueFromAsset<Vector4, Vector4ValueAsset>, IVector4Value
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector4 values from an assigned Vector4ListValueAsset.
    /// </summary>
    [Serializable]
    public class Vector4ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector4>, Vector4ListValueAsset>
    {
    }
}
