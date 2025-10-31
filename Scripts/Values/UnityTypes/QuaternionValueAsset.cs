using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Quaternion value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion value", fileName = "QuaternionValue")]
    public class QuaternionValueAsset : ValueAsset<Quaternion>
    {
        [SerializeField]
        private Quaternion value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Quaternion Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Quaternion value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Quaternion values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion list value", fileName = "QuaternionListValue")]
    public class QuaternionListValueAsset : SerializedValueListAsset<Quaternion>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Quaternion value from the assigned QuaternionValueAsset.
    /// </summary>
    [Serializable]
    public class QuaternionValueFromAsset : ValueFromAsset<Quaternion, QuaternionValueAsset>, IQuaternionValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Quaternion values from an assigned QuaternionListValueAsset.
    /// </summary>
    [Serializable]
    public class QuaternionListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Quaternion>, QuaternionListValueAsset>
    {
    }
}
