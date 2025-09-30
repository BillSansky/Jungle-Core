using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class LongValueComponent : ValueComponent<long>
    {
        [SerializeField]
        private long value;

        public override long GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class LongValueFromComponent : ValueFromComponent<long, LongValueComponent>, ILongValue
    {
    }
}
