using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a LayerMask value.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/LayerMask value", fileName = "LayerMaskValue")]
    [JungleClassInfo("Layer Mask Value Asset", "ScriptableObject storing a LayerMask value.", null, "Game Dev")]
    public class LayerMaskValueAsset : ValueAsset<LayerMask>
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
    /// ScriptableObject storing a list of LayerMask values.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/LayerMask list value", fileName = "LayerMaskListValue")]
    [JungleClassInfo("Layer Mask List Asset", "ScriptableObject storing a list of LayerMask values.", null, "Game Dev")]
    public class LayerMaskListValueAsset : SerializedValueListAsset<LayerMask>
    {
    }
    /// <summary>
    /// Reads a LayerMask value from a LayerMaskValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Layer Mask Value From Asset", "Reads a LayerMask value from a LayerMaskValueAsset.", null, "Game Dev")]
    public class LayerMaskValueFromAsset :
        ValueFromAsset<LayerMask, LayerMaskValueAsset>, ISettableLayerMaskValue
    {
    }
    /// <summary>
    /// Reads LayerMask values from a LayerMaskListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Layer Mask List From Asset", "Reads LayerMask values from a LayerMaskListValueAsset.", null, "Game Dev")]
    public class LayerMaskListValueFromAsset :
        ValueFromAsset<IReadOnlyList<LayerMask>, LayerMaskListValueAsset>
    {
    }
}
