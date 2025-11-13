using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Represents a value provider that returns a Vector3 value.
    /// </summary>
    public interface IVector3Value : IValue<Vector3>
    {
    }
    public interface ISettableVector3Value : IVector3Value, IValueSableValue<Vector3>
    {
    }
    /// <summary>
    /// Stores a 3D vector locally on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector3 Value", "Stores a 3D vector locally on the owner.", null, "Values/Unity Primitives", true)]
    public class Vector3Value : LocalValue<Vector3>, ISettableVector3Value
    {
        /// <summary>
        /// Initializes a new instance of the Vector3Value.
        /// </summary>
        public Vector3Value()
        {
        }
        /// <summary>
        /// Initializes a new instance of the Vector3Value.
        /// </summary>

        public Vector3Value(Vector3 value)
            : base(value)
        {
        }
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>

        public override bool HasMultipleValues => false;
    }
    /// <summary>
    /// Returns a 3D vector from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Vector3 Member Value", "Returns a 3D vector from a component field, property, or method.", null, "Values/Unity Primitives")]
    public class Vector3ClassMembersValue : ClassMembersValue<Vector3>, IVector3Value
    {
    }
}
