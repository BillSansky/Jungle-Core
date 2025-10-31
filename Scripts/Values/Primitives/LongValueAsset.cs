using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Long value", fileName = "LongValue")]
    [JungleClassInfo("Long Value Asset", "ScriptableObject storing a 64-bit integer.", null, "Values/Primitives")]
    public class LongValueAsset : ValueAsset<long>
    {
        [SerializeField]
        private long value;

        public override long Value()
        {
            return value;
        }

        public override void SetValue(long value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Long list value", fileName = "LongListValue")]
    [JungleClassInfo("Long List Asset", "ScriptableObject storing a list of 64-bit integers.", null, "Values/Primitives")]
    public class LongListValueAsset : SerializedValueListAsset<long>
    {
    }

    [Serializable]
    [JungleClassInfo("Long Value From Asset", "Reads a 64-bit integer from a LongValueAsset.", null, "Values/Primitives")]
    public class LongValueFromAsset : ValueFromAsset<long, LongValueAsset>, ILongValue
    {
    }

    [Serializable]
    [JungleClassInfo("Long List From Asset", "Reads 64-bit integers from a LongListValueAsset.", null, "Values/Primitives")]
    public class LongListValueFromAsset : ValueFromAsset<IReadOnlyList<long>, LongListValueAsset>
    {
    }
}
