using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Long value", fileName = "LongValue")]
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

    [Serializable]
    public class LongValueFromAsset : ValueFromAsset<long, LongValueAsset>, ILongValue
    {
    }
}
