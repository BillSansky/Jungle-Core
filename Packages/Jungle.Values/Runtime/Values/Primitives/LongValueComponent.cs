using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Component exposing a 64-bit integer.
    /// </summary>
    [JungleClassInfo("Long Value Component", "Component exposing a 64-bit integer.", null, "Primitives")]
    public class LongValueComponent : ValueComponent<long>
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
    /// Component exposing a list of 64-bit integers.
    /// </summary>

    [JungleClassInfo("Long List Component", "Component exposing a list of 64-bit integers.", null, "Primitives")]
    public class LongListValueComponent : SerializedValueListComponent<long>
    {
    }
    /// <summary>
    /// Reads a 64-bit integer from a LongValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Long Value From Component", "Reads a 64-bit integer from a LongValueComponent.", null, "Primitives")]
    public class LongValueFromComponent : ValueFromComponent<long, LongValueComponent>, ISettableLongValue
    {
    }
    /// <summary>
    /// Reads 64-bit integers from a LongListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Long List From Component", "Reads 64-bit integers from a LongListValueComponent.", null, "Primitives")]
    public class LongListValueFromComponent : ValueFromComponent<IReadOnlyList<long>, LongListValueComponent>
    {
    }
}
