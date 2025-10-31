using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Vector2 value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class Vector2ValueComponent : ValueComponent<Vector2>
    {
        [SerializeField]
        private Vector2 value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Vector2 Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Vector2 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Vector2 values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class Vector2ListValueComponent : SerializedValueListComponent<Vector2>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector2 value from the assigned Vector2ValueComponent.
    /// </summary>
    [Serializable]
    public class Vector2ValueFromComponent : ValueFromComponent<Vector2, Vector2ValueComponent>, IVector2Value
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector2 values from a Vector2ListValueComponent component.
    /// </summary>
    [Serializable]
    public class Vector2ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector2>, Vector2ListValueComponent>
    {
    }
}
