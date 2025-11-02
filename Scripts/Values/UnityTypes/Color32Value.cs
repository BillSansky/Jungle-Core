using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IColor32Value : IValue<Color32>
    {
    }

    [Serializable]
    [JungleClassInfo("Color32 Value", "Stores a Color32 value locally on the owner.", null, "Values/Unity Types", true)]
    public class Color32Value : LocalValue<Color32>, IColor32Value
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Color32 Member Value", "Returns a Color32 value from a component field, property, or method.", null, "Values/Unity Types")]
    public class Color32ClassMembersValue : ClassMembersValue<Color32>, IColor32Value
    {
    }
}
