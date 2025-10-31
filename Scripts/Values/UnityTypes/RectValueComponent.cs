using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Rect value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class RectValueComponent : ValueComponent<Rect>
    {
        [SerializeField]
        private Rect value = new Rect(0f, 0f, 1f, 1f);
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Rect Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Rect value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Rect values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class RectListValueComponent : SerializedValueListComponent<Rect>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Rect value from the assigned RectValueComponent.
    /// </summary>
    [Serializable]
    public class RectValueFromComponent : ValueFromComponent<Rect, RectValueComponent>, IRectValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Rect values from a RectListValueComponent component.
    /// </summary>
    [Serializable]
    public class RectListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Rect>, RectListValueComponent>
    {
    }
}
