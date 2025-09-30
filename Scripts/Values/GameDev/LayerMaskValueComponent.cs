using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class LayerMaskValueComponent : ValueComponent<LayerMask>
    {
        [SerializeField]
        private LayerMask value;

        public override LayerMask GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class LayerMaskValueFromComponent :
        ValueFromComponent<LayerMask, LayerMaskValueComponent>, ILayerMaskValue
    {
    }
}
