using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IVector2IntValue contract.
    /// </summary>
    public interface IVector2IntValue : IValue<Vector2Int>
    {
    }
    /// <summary>
    /// Stores a Vector2Int value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class Vector2IntValue : LocalValue<Vector2Int>, IVector2IntValue
    {
        public override bool HasMultipleValues => false;
        
        
    }
    /// <summary>
    /// Resolves a Vector2Int value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class Vector2IntClassMembersValue : ClassMembersValue<Vector2Int>, IVector2IntValue
    {
    }
}
