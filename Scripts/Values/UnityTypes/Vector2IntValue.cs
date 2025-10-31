using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector2IntValue : IValue<Vector2Int>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector2Int Value", "Stores a 2D integer vector locally on the owner.", null, "Values/Unity Types", true)]
    public class Vector2IntValue : LocalValue<Vector2Int>, IVector2IntValue
    {
        public override bool HasMultipleValues => false;
        
        
    }

    [Serializable]
    [JungleClassInfo("Vector2Int Member Value", "Returns a 2D integer vector from a component field, property, or method.", null, "Values/Unity Types")]
    public class Vector2IntClassMembersValue : ClassMembersValue<Vector2Int>, IVector2IntValue
    {
    }
}
