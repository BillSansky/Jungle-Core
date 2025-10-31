using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Defines the IVector3Value contract.
    /// </summary>
    public interface IVector3Value : IValue<Vector3>
    {
    }
    /// <summary>
    /// Stores a Vector3 value directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class Vector3Value : LocalValue<Vector3>, IVector3Value
    {
        public Vector3Value()
        {
        }

        public Vector3Value(Vector3 value)
            : base(value)
        {
        }

        public override bool HasMultipleValues => false;
    }
    /// <summary>
    /// Resolves a Vector3 value by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class Vector3ClassMembersValue : ClassMembersValue<Vector3>, IVector3Value
    {
    }
}
