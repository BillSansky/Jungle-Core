using System;
using System.Collections.Generic;
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

    public class LongListValueComponent : SerializedValueListComponent<long>
    {
    }

    [Serializable]
    public class LongValueFromComponent : ValueFromComponent<long, LongValueComponent>, ILongValue
    {
    }

    [Serializable]
    public class LongListValueFromComponent : ValueFromComponent<IReadOnlyList<long>, LongListValueComponent>
    {
    }
}
