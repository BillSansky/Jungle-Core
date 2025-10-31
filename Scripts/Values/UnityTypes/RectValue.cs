using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IRectValue contract.
    /// </summary>
    public interface IRectValue : IValue<Rect>
    {
    }
    /// <summary>
    /// Stores a Rect value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class RectValue : LocalValue<Rect>, IRectValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Rect value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class RectClassMembersValue : ClassMembersValue<Rect>, IRectValue
    {
    }
}
