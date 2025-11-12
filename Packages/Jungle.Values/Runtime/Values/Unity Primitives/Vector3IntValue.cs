using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Vector3Int value.
    /// </summary>
    public interface IVector3IntValue : IValue<Vector3Int>
    {
    }
    public interface ISettableVector3IntValue : IVector3IntValue, IValueSableValue<Vector3Int>
    {
    }
    /// <summary>
    /// Stores a 3D integer vector locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector3Int Value", "Stores a 3D integer vector locally on the owner.", null, "Values/Unity Primitives", true)]
    public class Vector3IntValue : LocalValue<Vector3Int>, ISettableVector3IntValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a 3D integer vector from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector3Int Member Value", "Returns a 3D integer vector from a component field, property, or method.", null, "Values/Unity Primitives")]
    public class Vector3IntClassMembersValue : ClassMembersValue<Vector3Int>, IVector3IntValue
    {
    }
}
