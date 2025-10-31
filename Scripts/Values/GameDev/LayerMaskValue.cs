using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the ILayerMaskValue contract.
    /// </summary>
    public interface ILayerMaskValue : IValue<LayerMask>
    {
    }
    /// <summary>
    /// Stores a LayerMask value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class LayerMaskValue : LocalValue<LayerMask>, ILayerMaskValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a LayerMask value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class LayerMaskClassMembersValue : ClassMembersValue<LayerMask>, ILayerMaskValue
    {
    }
}
