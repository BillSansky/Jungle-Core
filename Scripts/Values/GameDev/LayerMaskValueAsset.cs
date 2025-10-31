using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a LayerMask value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/LayerMask value", fileName = "LayerMaskValue")]
    public class LayerMaskValueAsset : ValueAsset<LayerMask>
    {
        [SerializeField]
        private LayerMask value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override LayerMask Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(LayerMask value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of LayerMask values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/LayerMask list value", fileName = "LayerMaskListValue")]
    public class LayerMaskListValueAsset : SerializedValueListAsset<LayerMask>
    {
    }
    /// <summary>
    /// Value wrapper that reads a LayerMask value from an assigned LayerMaskValueAsset.
    /// </summary>
    [Serializable]
    public class LayerMaskValueFromAsset :
        ValueFromAsset<LayerMask, LayerMaskValueAsset>, ILayerMaskValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of LayerMask values from an assigned LayerMaskListValueAsset.
    /// </summary>
    [Serializable]
    public class LayerMaskListValueFromAsset :
        ValueFromAsset<IReadOnlyList<LayerMask>, LayerMaskListValueAsset>
    {
    }
}
