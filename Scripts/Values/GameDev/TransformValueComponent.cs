using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes a Transform reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class TransformValueComponent : ValueComponent<Transform>
    {
        [SerializeField]
        private Transform value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Transform Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Transform value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Transform references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class TransformListValueComponent : SerializedValueListComponent<Transform>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Transform reference from a TransformValueComponent component.
    /// </summary>
    [Serializable]
    public class TransformValueFromComponent :
        ValueFromComponent<Transform, TransformValueComponent>, ITransformValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Transform references from a TransformListValueComponent component.
    /// </summary>
    [Serializable]
    public class TransformListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Transform>, TransformListValueComponent>
    {
    }
}
