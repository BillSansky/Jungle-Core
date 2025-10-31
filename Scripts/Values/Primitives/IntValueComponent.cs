using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// MonoBehaviour that serializes an int value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class IntValueComponent : ValueComponent<int>
    {
        [SerializeField]
        private int value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override int Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of int values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class IntListValueComponent : SerializedValueListComponent<int>
    {
    }
    /// <summary>
    /// Value wrapper that reads an int value from the assigned IntValueComponent.
    /// </summary>
    [Serializable]
    public class IntValueFromComponent : ValueFromComponent<int, IntValueComponent>, IIntValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of int values from the assigned IntListValueComponent.
    /// </summary>
    [Serializable]
    public class IntListValueFromComponent : ValueFromComponent<IReadOnlyList<int>, IntListValueComponent>
    {
    }
}
