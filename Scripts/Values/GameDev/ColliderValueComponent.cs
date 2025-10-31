using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes a Collider reference so scene objects can expose it to Jungle systems.
    /// </summary>
    [Serializable]
    public class ColliderValueComponent : ValueComponent<Collider>
    {
        [SerializeField]
        private Collider value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Collider Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Collider value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Collider references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class ColliderListValueComponent : SerializedValueListComponent<Collider>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Collider reference from a ColliderValueComponent component.
    /// </summary>
    [Serializable]
    public class ColliderValueFromComponent :
        ValueFromComponent<Collider, ColliderValueComponent>, IColliderValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Collider references from a ColliderListValueComponent component.
    /// </summary>
    [Serializable]
    public class ColliderListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Collider>, ColliderListValueComponent>
    {
    }
}
