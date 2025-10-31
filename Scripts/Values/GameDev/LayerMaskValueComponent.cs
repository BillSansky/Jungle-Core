using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// MonoBehaviour that serializes a LayerMask value so scene objects can expose it to Jungle systems.
    /// </summary>
    public class LayerMaskValueComponent : ValueComponent<LayerMask>
    {
        [SerializeField]
        private LayerMask value;
        /// <summary>
        /// Returns the serialized value provided by the component.
        /// </summary>
        public override LayerMask Value()
        {
            return value;
        }
        /// <summary>
        /// Updates the serialized value stored on the component.
        /// </summary>
        public override void SetValue(LayerMask value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// MonoBehaviour that serializes a list of LayerMask values so scene objects can expose them to Jungle systems.
    /// </summary>
    public class LayerMaskListValueComponent : SerializedValueListComponent<LayerMask>
    {
    }
    /// <summary>
    /// Value wrapper that reads a LayerMask value from a LayerMaskValueComponent component.
    /// </summary>
    [Serializable]
    public class LayerMaskValueFromComponent :
        ValueFromComponent<LayerMask, LayerMaskValueComponent>, ILayerMaskValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of LayerMask values from a LayerMaskListValueComponent component.
    /// </summary>
    [Serializable]
    public class LayerMaskListValueFromComponent :
        ValueFromComponent<IReadOnlyList<LayerMask>, LayerMaskListValueComponent>
    {
    }
}
