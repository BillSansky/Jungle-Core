using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IVector2Value : IValue<Vector2>
    {
    }

    [Serializable]
    [JungleClassInfo("Vector2 Value", "Stores a 2D vector locally on the owner.", null, "Values/Unity Types", true)]
    public class Vector2Value : LocalValue<Vector2>, IVector2Value
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Vector2 Member Value", "Returns a 2D vector from a component field, property, or method.", null, "Values/Unity Types")]
    public class Vector2ClassMembersValue : ClassMembersValue<Vector2>, IVector2Value
    {
    }
}
