using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class LongValueComponent : ValueComponent<long>
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
    public class LongValueFromComponent : ValueFromComponent<long, LongValueComponent>, ILongValue
    {
    }
}
