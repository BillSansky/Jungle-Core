using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes a Rigidbody reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class RigidbodyValueComponent : ValueComponent<Rigidbody>
    {
        [SerializeField]
        private Rigidbody value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Rigidbody Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Rigidbody value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Rigidbody references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class RigidbodyListValueComponent : SerializedValueListComponent<Rigidbody>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Rigidbody reference from a RigidbodyValueComponent component.
    /// </summary>
    [Serializable]
    public class RigidbodyValueFromComponent :
        ValueFromComponent<Rigidbody, RigidbodyValueComponent>, IRigidbodyValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Rigidbody references from a RigidbodyListValueComponent component.
    /// </summary>
    [Serializable]
    public class RigidbodyListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Rigidbody>, RigidbodyListValueComponent>
    {
    }
}
