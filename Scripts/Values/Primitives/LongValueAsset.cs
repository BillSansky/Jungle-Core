using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a long value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Long value", fileName = "LongValue")]
    public class LongValueAsset : ValueAsset<long>
    {
        [SerializeField]
        private long value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override long Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(long value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of long values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Long list value", fileName = "LongListValue")]
    public class LongListValueAsset : SerializedValueListAsset<long>
    {
    }
    /// <summary>
    /// Value wrapper that reads a long value from the assigned LongValueAsset.
    /// </summary>
    [Serializable]
    public class LongValueFromAsset : ValueFromAsset<long, LongValueAsset>, ILongValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of long values from the assigned LongListValueAsset.
    /// </summary>
    [Serializable]
    public class LongListValueFromAsset : ValueFromAsset<IReadOnlyList<long>, LongListValueAsset>
    {
    }
}
