using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// MonoBehaviour that serializes a double value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class DoubleValueComponent : ValueComponent<double>
    {
        [SerializeField]
        private double value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override double Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(double value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of double values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class DoubleListValueComponent : SerializedValueListComponent<double>
    {
    }
    /// <summary>
    /// Value wrapper that reads a double value from the assigned DoubleValueComponent.
    /// </summary>
    [Serializable]
    public class DoubleValueFromComponent : ValueFromComponent<double, DoubleValueComponent>, IDoubleValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of double values from the assigned DoubleListValueComponent.
    /// </summary>
    [Serializable]
    public class DoubleListValueFromComponent : ValueFromComponent<IReadOnlyList<double>, DoubleListValueComponent>
    {
    }
}
