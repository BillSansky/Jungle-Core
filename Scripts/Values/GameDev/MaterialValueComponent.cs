using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes a Material reference so scene objects can expose it to Jungle systems.
    /// </summary>
    public class MaterialValueComponent : ValueComponent<Material>
    {
        [SerializeField]
        private Material value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override Material Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(Material value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of Material references so scene objects can expose them to Jungle systems.
    /// </summary>
    public class MaterialListValueComponent : SerializedValueListComponent<Material>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Material reference from a MaterialValueComponent component.
    /// </summary>
    [Serializable]
    public class MaterialValueFromComponent :
        ValueFromComponent<Material, MaterialValueComponent>, IMaterialValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Material references from a MaterialListValueComponent component.
    /// </summary>
    [Serializable]
    public class MaterialListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Material>, MaterialListValueComponent>
    {
    }
}
