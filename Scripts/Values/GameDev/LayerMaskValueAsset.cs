using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/LayerMask value", fileName = "LayerMaskValue")]
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
    public class LayerMaskListValueAsset : SerializedValueListAsset<LayerMask>
    {
    }

    [Serializable]
    public class LayerMaskValueFromAsset :
        ValueFromAsset<LayerMask, LayerMaskValueAsset>, ILayerMaskValue
    {
    }

    [Serializable]
    public class LayerMaskListValueFromAsset :
        ValueFromAsset<IReadOnlyList<LayerMask>, LayerMaskListValueAsset>
    {
    }
}
