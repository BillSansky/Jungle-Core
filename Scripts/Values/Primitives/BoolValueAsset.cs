using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a boolean value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Bool value", fileName = "BoolValue")]
    public class BoolValueAsset : ValueAsset<bool>
    {
        [SerializeField]
        private bool value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override bool Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(bool value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of boolean values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Bool list value", fileName = "BoolListValue")]
    public class BoolListValueAsset : SerializedValueListAsset<bool>
    {
    }
    /// <summary>
    /// Value wrapper that reads a boolean value from the assigned BoolValueAsset.
    /// </summary>
    [Serializable]
    public class BoolValueFromAsset : ValueFromAsset<bool, BoolValueAsset>, IBoolValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of boolean values from the assigned BoolListValueAsset.
    /// </summary>
    [Serializable]
    public class BoolListValueFromAsset : ValueFromAsset<IReadOnlyList<bool>, BoolListValueAsset>
    {
    }
}
