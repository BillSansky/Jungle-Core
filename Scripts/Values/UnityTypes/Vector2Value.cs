using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IVector2Value contract.
    /// </summary>
    public interface IVector2Value : IValue<Vector2>
    {
    }
    /// <summary>
    /// Stores a Vector2 value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class Vector2Value : LocalValue<Vector2>, IVector2Value
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Vector2 value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class Vector2ClassMembersValue : ClassMembersValue<Vector2>, IVector2Value
    {
    }
}
