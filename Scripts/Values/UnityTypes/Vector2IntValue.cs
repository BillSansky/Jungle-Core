using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector2IntValue : IValue<Vector2Int>
    {
    }

    [Serializable]
    public class Vector2IntValue : LocalValue<Vector2Int>, IVector2IntValue
    {
    }
}
