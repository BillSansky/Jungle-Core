using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Represents a value provider that returns a LayerMask value.
    /// </summary>
    public interface ILayerMaskValue : IValue<LayerMask>
    {
    }
    /// <summary>
    /// Stores a LayerMask value directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Layer Mask Value", "Stores a LayerMask value directly on the owner.", null, "Values/Game Dev", true)]
    public class LayerMaskValue : LocalValue<LayerMask>, ILayerMaskValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a LayerMask value from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Layer Mask Member Value", "Returns a LayerMask value from a component field, property, or method.", null, "Values/Game Dev")]
    public class LayerMaskClassMembersValue : ClassMembersValue<LayerMask>, ILayerMaskValue
    {
    }
}
