using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ILayerMaskValue : IValue<LayerMask>
    {
    }

    [Serializable]
    public class LayerMaskValue : LocalValue<LayerMask>, ILayerMaskValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
