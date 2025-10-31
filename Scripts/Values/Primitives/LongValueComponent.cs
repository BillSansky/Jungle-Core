using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [JungleClassInfo("Long Value Component", "Component exposing a 64-bit integer.", null, "Values/Primitives")]
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

    [JungleClassInfo("Long List Component", "Component exposing a list of 64-bit integers.", null, "Values/Primitives")]
    public class LongListValueComponent : SerializedValueListComponent<long>
    {
    }

    [Serializable]
    [JungleClassInfo("Long Value From Component", "Reads a 64-bit integer from a LongValueComponent.", null, "Values/Primitives")]
    public class LongValueFromComponent : ValueFromComponent<long, LongValueComponent>, ILongValue
    {
    }

    [Serializable]
    [JungleClassInfo("Long List From Component", "Reads 64-bit integers from a LongListValueComponent.", null, "Values/Primitives")]
    public class LongListValueFromComponent : ValueFromComponent<IReadOnlyList<long>, LongListValueComponent>
    {
    }
}
