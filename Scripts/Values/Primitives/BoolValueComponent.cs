using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// MonoBehaviour that serializes a boolean value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class BoolValueComponent : ValueComponent<bool>
    {
        [SerializeField]
        private bool value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override bool Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(bool value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of boolean values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class BoolListValueComponent : SerializedValueListComponent<bool>
    {
    }
    /// <summary>
    /// Value wrapper that reads a boolean value from the assigned BoolValueComponent.
    /// </summary>
    [Serializable]
    public class BoolValueFromComponent : ValueFromComponent<bool, BoolValueComponent>, IBoolValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of boolean values from the assigned BoolListValueComponent.
    /// </summary>
    [Serializable]
    public class BoolListValueFromComponent : ValueFromComponent<IReadOnlyList<bool>, BoolListValueComponent>
    {
    }
}
