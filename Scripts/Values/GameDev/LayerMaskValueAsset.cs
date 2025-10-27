using System;
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

    [Serializable]
    public class LayerMaskValueFromAsset :
        ValueFromAsset<LayerMask, LayerMaskValueAsset>, ILayerMaskValue
    {
    }
}
