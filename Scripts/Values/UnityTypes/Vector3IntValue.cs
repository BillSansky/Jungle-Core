using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IVector3IntValue contract.
    /// </summary>
    public interface IVector3IntValue : IValue<Vector3Int>
    {
    }
    /// <summary>
    /// Stores a Vector3Int value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class Vector3IntValue : LocalValue<Vector3Int>, IVector3IntValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Vector3Int value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class Vector3IntClassMembersValue : ClassMembersValue<Vector3Int>, IVector3IntValue
    {
    }
}
