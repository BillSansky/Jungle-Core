using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IColor32Value contract.
    /// </summary>
    public interface IColor32Value : IValue<Color32>
    {
    }
    /// <summary>
    /// Stores a Color32 value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class Color32Value : LocalValue<Color32>, IColor32Value
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Color32 value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class Color32ClassMembersValue : ClassMembersValue<Color32>, IColor32Value
    {
    }
}
