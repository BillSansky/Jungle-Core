using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector2IntValue : IValue<Vector2Int>
    {
    }

    [Serializable]
    public class Vector2IntValue : LocalValue<Vector2Int>, IVector2IntValue
    {
        public override bool HasMultipleValues => false;
        
        
    }

    [Serializable]
    public class Vector2IntClassMembersValue : ClassMembersValue<Vector2Int>, IVector2IntValue
    {
    }
}
