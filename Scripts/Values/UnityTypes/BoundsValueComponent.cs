using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Bounds value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class BoundsValueComponent : ValueComponent<Bounds>
    {
        [SerializeField]
        private Bounds value = new Bounds(Vector3.zero, Vector3.one);
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Bounds Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Bounds value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Bounds values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class BoundsListValueComponent : SerializedValueListComponent<Bounds>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Bounds value from the assigned BoundsValueComponent.
    /// </summary>
    [Serializable]
    public class BoundsValueFromComponent : ValueFromComponent<Bounds, BoundsValueComponent>, IBoundsValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Bounds values from a BoundsListValueComponent component.
    /// </summary>
    [Serializable]
    public class BoundsListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Bounds>, BoundsListValueComponent>
    {
    }
}
