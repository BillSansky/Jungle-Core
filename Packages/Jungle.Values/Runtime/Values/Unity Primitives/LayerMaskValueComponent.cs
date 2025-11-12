using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a LayerMask value.
    /// </summary>
    [JungleClassInfo("Layer Mask Value Component", "Component exposing a LayerMask value.", null, "Game Dev")]
    public class LayerMaskValueComponent : ValueComponent<LayerMask>
    {
        [SerializeField]
        private LayerMask value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override LayerMask Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(LayerMask value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of LayerMask values.
    /// </summary>

    [JungleClassInfo("Layer Mask List Component", "Component exposing a list of LayerMask values.", null, "Game Dev")]
    public class LayerMaskListValueComponent : SerializedValueListComponent<LayerMask>
    {
    }
    /// <summary>
    /// Reads a LayerMask value from a LayerMaskValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Layer Mask Value From Component", "Reads a LayerMask value from a LayerMaskValueComponent.", null, "Game Dev")]
    public class LayerMaskValueFromComponent :
        ValueFromComponent<LayerMask, LayerMaskValueComponent>, ISettableLayerMaskValue
    {
    }
    /// <summary>
    /// Reads LayerMask values from a LayerMaskListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Layer Mask List From Component", "Reads LayerMask values from a LayerMaskListValueComponent.", null, "Game Dev")]
    public class LayerMaskListValueFromComponent :
        ValueFromComponent<IReadOnlyList<LayerMask>, LayerMaskListValueComponent>
    {
    }
}
