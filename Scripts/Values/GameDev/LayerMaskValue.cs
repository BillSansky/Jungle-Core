using System;
using Jungle.Values;
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

    [Serializable]
    public class LayerMaskClassMembersValue : ClassMembersValue<LayerMask>, ILayerMaskValue
    {
    }
}
