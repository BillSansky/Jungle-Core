using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Vector3Int value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3Int value", fileName = "Vector3IntValue")]
    public class Vector3IntValueAsset : ValueAsset<Vector3Int>
    {
        [SerializeField]
        private Vector3Int value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Vector3Int Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Vector3Int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Vector3Int values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3Int list value", fileName = "Vector3IntListValue")]
    public class Vector3IntListValueAsset : SerializedValueListAsset<Vector3Int>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector3Int value from an assigned Vector3IntValueAsset.
    /// </summary>
    [Serializable]
    public class Vector3IntValueFromAsset :
        ValueFromAsset<Vector3Int, Vector3IntValueAsset>, IVector3IntValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector3Int values from an assigned Vector3IntListValueAsset.
    /// </summary>
    [Serializable]
    public class Vector3IntListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector3Int>, Vector3IntListValueAsset>
    {
    }
}
