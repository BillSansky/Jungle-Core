using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a text string.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/String value", fileName = "StringValue")]
    [JungleClassInfo("String Value Asset", "ScriptableObject storing a text string.", null, "Values/Primitives")]
    public class StringValueAsset : ValueAsset<string>
    {
        [SerializeField]
        private string value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override string Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(string value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of text strings.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/String list value", fileName = "StringListValue")]
    [JungleClassInfo("String List Asset", "ScriptableObject storing a list of text strings.", null, "Values/Primitives")]
    public class StringListValueAsset : SerializedValueListAsset<string>
    {
    }
    /// <summary>
    /// Reads a text string from a StringValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("String Value From Asset", "Reads a text string from a StringValueAsset.", null, "Values/Primitives")]
    public class StringValueFromAsset : ValueFromAsset<string, StringValueAsset>, ISettableStringValue
    {
    }
    /// <summary>
    /// Reads text strings from a StringListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("String List From Asset", "Reads text strings from a StringListValueAsset.", null, "Values/Primitives")]
    public class StringListValueFromAsset : ValueFromAsset<IReadOnlyList<string>, StringListValueAsset>
    {
    }
}
