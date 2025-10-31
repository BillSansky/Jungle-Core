using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/LayerMask value", fileName = "LayerMaskValue")]
    [JungleClassInfo("Layer Mask Value Asset", "ScriptableObject storing a LayerMask value.", null, "Values/Game Dev")]
    public class LayerMaskValueAsset : ValueAsset<LayerMask>
    {
        [SerializeField]
        private LayerMask value;

        public override LayerMask Value()
        {
            return value;
        }

        public override void SetValue(LayerMask value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/LayerMask list value", fileName = "LayerMaskListValue")]
    [JungleClassInfo("Layer Mask List Asset", "ScriptableObject storing a list of LayerMask values.", null, "Values/Game Dev")]
    public class LayerMaskListValueAsset : SerializedValueListAsset<LayerMask>
    {
    }

    [Serializable]
    [JungleClassInfo("Layer Mask Value From Asset", "Reads a LayerMask value from a LayerMaskValueAsset.", null, "Values/Game Dev")]
    public class LayerMaskValueFromAsset :
        ValueFromAsset<LayerMask, LayerMaskValueAsset>, ILayerMaskValue
    {
    }

    [Serializable]
    [JungleClassInfo("Layer Mask List From Asset", "Reads LayerMask values from a LayerMaskListValueAsset.", null, "Values/Game Dev")]
    public class LayerMaskListValueFromAsset :
        ValueFromAsset<IReadOnlyList<LayerMask>, LayerMaskListValueAsset>
    {
    }
}
