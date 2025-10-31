using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Color32 value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class Color32ValueComponent : ValueComponent<Color32>
    {
        [SerializeField]
        private Color32 value = new Color32(255, 255, 255, 255);
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Color32 Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Color32 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Color32 values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class Color32ListValueComponent : SerializedValueListComponent<Color32>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Color32 value from the assigned Color32ValueComponent.
    /// </summary>
    [Serializable]
    public class Color32ValueFromComponent : ValueFromComponent<Color32, Color32ValueComponent>, IColor32Value
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Color32 values from a Color32ListValueComponent component.
    /// </summary>
    [Serializable]
    public class Color32ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Color32>, Color32ListValueComponent>
    {
    }
}
