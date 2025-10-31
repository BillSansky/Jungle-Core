using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Layer Mask Value Component", "Component exposing a LayerMask value.", null, "Values/Game Dev")]
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

    [JungleClassInfo("Layer Mask List Component", "Component exposing a list of LayerMask values.", null, "Values/Game Dev")]
    public class LayerMaskListValueComponent : SerializedValueListComponent<LayerMask>
    {
    }

    [Serializable]
    [JungleClassInfo("Layer Mask Value From Component", "Reads a LayerMask value from a LayerMaskValueComponent.", null, "Values/Game Dev")]
    public class LayerMaskValueFromComponent :
        ValueFromComponent<LayerMask, LayerMaskValueComponent>, ILayerMaskValue
    {
    }

    [Serializable]
    [JungleClassInfo("Layer Mask List From Component", "Reads LayerMask values from a LayerMaskListValueComponent.", null, "Values/Game Dev")]
    public class LayerMaskListValueFromComponent :
        ValueFromComponent<IReadOnlyList<LayerMask>, LayerMaskListValueComponent>
    {
    }
}
