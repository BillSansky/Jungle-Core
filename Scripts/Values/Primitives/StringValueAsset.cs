using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a string value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/String value", fileName = "StringValue")]
    public class StringValueAsset : ValueAsset<string>
    {
        [SerializeField]
        private string value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override string Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(string value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of string values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/String list value", fileName = "StringListValue")]
    public class StringListValueAsset : SerializedValueListAsset<string>
    {
    }
    /// <summary>
    /// Value wrapper that reads a string value from the assigned StringValueAsset.
    /// </summary>
    [Serializable]
    public class StringValueFromAsset : ValueFromAsset<string, StringValueAsset>, IStringValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of string values from the assigned StringListValueAsset.
    /// </summary>
    [Serializable]
    public class StringListValueFromAsset : ValueFromAsset<IReadOnlyList<string>, StringListValueAsset>
    {
    }
}
