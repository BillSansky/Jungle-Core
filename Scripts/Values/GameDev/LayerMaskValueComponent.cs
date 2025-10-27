using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class LayerMaskValueComponent : ValueComponent<LayerMask>
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
    public class LayerMaskValueFromComponent :
        ValueFromComponent<LayerMask, LayerMaskValueComponent>, ILayerMaskValue
    {
    }
}
