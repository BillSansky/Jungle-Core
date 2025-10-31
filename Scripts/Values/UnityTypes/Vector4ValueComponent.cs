using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Vector4 value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class Vector4ValueComponent : ValueComponent<Vector4>
    {
        [SerializeField]
        private Vector4 value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Vector4 Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Vector4 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Vector4 values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class Vector4ListValueComponent : SerializedValueListComponent<Vector4>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector4 value from the assigned Vector4ValueComponent.
    /// </summary>
    [Serializable]
    public class Vector4ValueFromComponent : ValueFromComponent<Vector4, Vector4ValueComponent>, IVector4Value
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector4 values from a Vector4ListValueComponent component.
    /// </summary>
    [Serializable]
    public class Vector4ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector4>, Vector4ListValueComponent>
    {
    }
}
