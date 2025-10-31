using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ILayerMaskValue : IValue<LayerMask>
    {
    }

    [Serializable]
    [JungleClassInfo("Layer Mask Value", "Stores a LayerMask value directly on the owner.", null, "Values/Game Dev", true)]
    public class LayerMaskValue : LocalValue<LayerMask>, ILayerMaskValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Layer Mask Member Value", "Returns a LayerMask value from a component field, property, or method.", null, "Values/Game Dev")]
    public class LayerMaskClassMembersValue : ClassMembersValue<LayerMask>, ILayerMaskValue
    {
    }
}
