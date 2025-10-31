using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector3Value : IValue<Vector3>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector3 Value", "Stores a 3D vector locally on the owner.", null, "Values/Unity Types", true)]
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

    [Serializable]
    [JungleClassInfo("Vector3 Member Value", "Returns a 3D vector from a component field, property, or method.", null, "Values/Unity Types")]
    public class Vector3ClassMembersValue : ClassMembersValue<Vector3>, IVector3Value
    {
    }
}
