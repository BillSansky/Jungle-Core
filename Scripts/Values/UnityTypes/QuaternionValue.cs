using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IQuaternionValue : IValue<Quaternion>
    {
    }

    [Serializable]
    public class QuaternionValue : LocalValue<Quaternion>, IQuaternionValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class QuaternionClassMembersValue : ClassMembersValue<Quaternion>, IQuaternionValue
    {
    }
}
