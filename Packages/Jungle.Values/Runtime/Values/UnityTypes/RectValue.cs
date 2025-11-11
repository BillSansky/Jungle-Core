using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Rect value.
    /// </summary>
    public interface IRectValue : IValue<Rect>
    {
    }
    /// <summary>
    /// Stores a Rect area locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rect Value", "Stores a Rect area locally on the owner.", null, "Values/Unity Types", true)]
    public class RectValue : LocalValue<Rect>, IRectValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a Rect area from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Rect Member Value", "Returns a Rect area from a component field, property, or method.", null, "Values/Unity Types")]
    public class RectClassMembersValue : ClassMembersValue<Rect>, IRectValue
    {
    }
}
