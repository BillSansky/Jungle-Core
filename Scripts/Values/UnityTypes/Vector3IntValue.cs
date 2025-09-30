using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector3IntValue : IValue<Vector3Int>
    {
    }

    [Serializable]
    public class Vector3IntValue : LocalValue<Vector3Int>, IVector3IntValue
    {
    }
}
