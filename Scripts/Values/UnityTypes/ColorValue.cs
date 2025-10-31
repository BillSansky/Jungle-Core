using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IColorValue contract.
    /// </summary>
    public interface IColorValue : IValue<Color>
    {
    }
    /// <summary>
    /// Stores a color value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class ColorValue : LocalValue<Color>, IColorValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a color value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class ColorClassMembersValue : ClassMembersValue<Color>, IColorValue
    {
    }
}
