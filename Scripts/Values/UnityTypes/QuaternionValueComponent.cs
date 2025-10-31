using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Quaternion value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class QuaternionValueComponent : ValueComponent<Quaternion>
    {
        [SerializeField]
        private Quaternion value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Quaternion Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Quaternion value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Quaternion values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class QuaternionListValueComponent : SerializedValueListComponent<Quaternion>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Quaternion value from the assigned QuaternionValueComponent.
    /// </summary>
    [Serializable]
    public class QuaternionValueFromComponent : ValueFromComponent<Quaternion, QuaternionValueComponent>,
        IQuaternionValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Quaternion values from a QuaternionListValueComponent component.
    /// </summary>
    [Serializable]
    public class QuaternionListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Quaternion>, QuaternionListValueComponent>
    {
    }
}
