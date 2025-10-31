using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// MonoBehaviour that serializes a long value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class LongValueComponent : ValueComponent<long>
    {
        [SerializeField]
        private long value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override long Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(long value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of long values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class LongListValueComponent : SerializedValueListComponent<long>
    {
    }
    /// <summary>
    /// Value wrapper that reads a long value from the assigned LongValueComponent.
    /// </summary>
    [Serializable]
    public class LongValueFromComponent : ValueFromComponent<long, LongValueComponent>, ILongValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of long values from the assigned LongListValueComponent.
    /// </summary>
    [Serializable]
    public class LongListValueFromComponent : ValueFromComponent<IReadOnlyList<long>, LongListValueComponent>
    {
    }
}
