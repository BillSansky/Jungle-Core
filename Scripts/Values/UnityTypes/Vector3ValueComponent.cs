using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Vector3 value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class Vector3ValueComponent : ValueComponent<Vector3>
    {
        [SerializeField]
        private Vector3 value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Vector3 Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Vector3 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Vector3 values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class Vector3ListValueComponent : SerializedValueListComponent<Vector3>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector3 value from the assigned Vector3ValueComponent.
    /// </summary>
    [Serializable]
    public class Vector3ValueFromComponent : ValueFromComponent<Vector3, Vector3ValueComponent>, IVector3Value
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector3 values from a Vector3ListValueComponent component.
    /// </summary>
    [Serializable]
    public class Vector3ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector3>, Vector3ListValueComponent>
    {
    }
}
