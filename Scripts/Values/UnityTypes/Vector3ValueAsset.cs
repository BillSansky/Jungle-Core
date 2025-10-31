using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Vector3 value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3 value", fileName = "Vector3Value")]
    public class Vector3ValueAsset : ValueAsset<Vector3>
    {
        [SerializeField]
        private Vector3 value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Vector3 Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Vector3 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Vector3 values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3 list value", fileName = "Vector3ListValue")]
    public class Vector3ListValueAsset : SerializedValueListAsset<Vector3>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector3 value from the assigned Vector3ValueAsset.
    /// </summary>
    [Serializable]
    public class Vector3ValueFromAsset : ValueFromAsset<Vector3, Vector3ValueAsset>, IVector3Value
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector3 values from an assigned Vector3ListValueAsset.
    /// </summary>
    [Serializable]
    public class Vector3ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector3>, Vector3ListValueAsset>
    {
    }
}
