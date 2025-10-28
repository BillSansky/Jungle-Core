using System;
using System.Collections.Generic;
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

    public class LayerMaskListValueComponent : SerializedValueListComponent<LayerMask>
    {
    }

    [Serializable]
    public class LayerMaskValueFromComponent :
        ValueFromComponent<LayerMask, LayerMaskValueComponent>, ILayerMaskValue
    {
    }

    [Serializable]
    public class LayerMaskListValueFromComponent :
        ValueFromComponent<IReadOnlyList<LayerMask>, LayerMaskListValueComponent>
    {
    }
}
