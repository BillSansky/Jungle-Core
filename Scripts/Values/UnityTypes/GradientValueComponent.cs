using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Gradient value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class GradientValueComponent : ValueComponent<Gradient>
    {
        [SerializeField]
        private Gradient value = new Gradient();
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Gradient Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Gradient value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Gradient values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class GradientListValueComponent : SerializedValueListComponent<Gradient>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Gradient value from the assigned GradientValueComponent.
    /// </summary>
    [Serializable]
    public class GradientValueFromComponent : ValueFromComponent<Gradient, GradientValueComponent>, IGradientValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Gradient values from a GradientListValueComponent component.
    /// </summary>
    [Serializable]
    public class GradientListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Gradient>, GradientListValueComponent>
    {
    }
}
