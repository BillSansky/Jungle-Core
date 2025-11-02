using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IQuaternionValue : IValue<Quaternion>
    {
    }

    [Serializable]
    [JungleClassInfo("Quaternion Value", "Stores a rotation quaternion locally on the owner.", null, "Values/Unity Types", true)]
    public class QuaternionValue : LocalValue<Quaternion>, IQuaternionValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Quaternion Member Value", "Returns a rotation quaternion from a component field, property, or method.", null, "Values/Unity Types")]
    public class QuaternionClassMembersValue : ClassMembersValue<Quaternion>, IQuaternionValue
    {
    }
}
