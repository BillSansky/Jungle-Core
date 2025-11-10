using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a 64-bit integer.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Long value", fileName = "LongValue")]
    [JungleClassInfo("Long Value Asset", "ScriptableObject storing a 64-bit integer.", null, "Values/Primitives")]
    public class LongValueAsset : ValueAsset<long>
    {
        [SerializeField]
        private long value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override long Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(long value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of 64-bit integers.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Long list value", fileName = "LongListValue")]
    [JungleClassInfo("Long List Asset", "ScriptableObject storing a list of 64-bit integers.", null, "Values/Primitives")]
    public class LongListValueAsset : SerializedValueListAsset<long>
    {
    }
    /// <summary>
    /// Reads a 64-bit integer from a LongValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Long Value From Asset", "Reads a 64-bit integer from a LongValueAsset.", null, "Values/Primitives")]
    public class LongValueFromAsset : ValueFromAsset<long, LongValueAsset>, ILongValue
    {
    }
    /// <summary>
    /// Reads 64-bit integers from a LongListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Long List From Asset", "Reads 64-bit integers from a LongListValueAsset.", null, "Values/Primitives")]
    public class LongListValueFromAsset : ValueFromAsset<IReadOnlyList<long>, LongListValueAsset>
    {
    }
}
