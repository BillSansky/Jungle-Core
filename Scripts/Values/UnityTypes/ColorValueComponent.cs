using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a color value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class ColorValueComponent : ValueComponent<Color>
    {
        [SerializeField]
        private Color value = Color.white;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Color Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Color value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of color values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class ColorListValueComponent : SerializedValueListComponent<Color>
    {
    }
    /// <summary>
    /// Value wrapper that reads a color value from the assigned ColorValueComponent.
    /// </summary>
    [Serializable]
    public class ColorValueFromComponent : ValueFromComponent<Color, ColorValueComponent>, IColorValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of color values from a ColorListValueComponent component.
    /// </summary>
    [Serializable]
    public class ColorListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Color>, ColorListValueComponent>
    {
    }
}
