using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IQuaternionValue : IValue<Quaternion>
    {
    }

    [Serializable]
    public class QuaternionValue : LocalValue<Quaternion>, IQuaternionValue
    {
    }
}
