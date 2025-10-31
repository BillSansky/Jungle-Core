using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// MonoBehaviour that serializes a float value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class FloatValueComponent : ValueComponent<float>
    {
        [SerializeField]
        private float value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override float Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(float value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of float values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class FloatListValueComponent : SerializedValueListComponent<float>
    {
    }
    /// <summary>
    /// Value wrapper that reads a float value from the assigned FloatValueComponent.
    /// </summary>
    [Serializable]
    public class FloatValueFromComponent : ValueFromComponent<float, FloatValueComponent>, IFloatValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of float values from the assigned FloatListValueComponent.
    /// </summary>
    [Serializable]
    public class FloatListValueFromComponent : ValueFromComponent<IReadOnlyList<float>, FloatListValueComponent>
    {
    }
}
