using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// MonoBehaviour that serializes a string value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class StringValueComponent : ValueComponent<string>
    {
        [SerializeField]
        private string value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override string Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(string value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of string values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class StringListValueComponent : SerializedValueListComponent<string>
    {
    }
    /// <summary>
    /// Value wrapper that reads a string value from the assigned StringValueComponent.
    /// </summary>
    [Serializable]
    public class StringValueFromComponent : ValueFromComponent<string, StringValueComponent>, IStringValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of string values from the assigned StringListValueComponent.
    /// </summary>
    [Serializable]
    public class StringListValueFromComponent : ValueFromComponent<IReadOnlyList<string>, StringListValueComponent>
    {
    }
}
