using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Vector2Int value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class Vector2IntValueComponent : ValueComponent<Vector2Int>
    {
        [SerializeField]
        private Vector2Int value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Vector2Int Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Vector2Int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Vector2Int values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class Vector2IntListValueComponent : SerializedValueListComponent<Vector2Int>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector2Int value from a Vector2IntValueComponent component.
    /// </summary>
    [Serializable]
    public class Vector2IntValueFromComponent :
        ValueFromComponent<Vector2Int, Vector2IntValueComponent>, IVector2IntValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector2Int values from a Vector2IntListValueComponent component.
    /// </summary>
    [Serializable]
    public class Vector2IntListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector2Int>, Vector2IntListValueComponent>
    {
    }
}
