using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// MonoBehaviour that serializes a Vector3Int value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class Vector3IntValueComponent : ValueComponent<Vector3Int>
    {
        [SerializeField]
        private Vector3Int value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Vector3Int Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Vector3Int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Vector3Int values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class Vector3IntListValueComponent : SerializedValueListComponent<Vector3Int>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Vector3Int value from a Vector3IntValueComponent component.
    /// </summary>
    [Serializable]
    public class Vector3IntValueFromComponent :
        ValueFromComponent<Vector3Int, Vector3IntValueComponent>, IVector3IntValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Vector3Int values from a Vector3IntListValueComponent component.
    /// </summary>
    [Serializable]
    public class Vector3IntListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector3Int>, Vector3IntListValueComponent>
    {
    }
}
