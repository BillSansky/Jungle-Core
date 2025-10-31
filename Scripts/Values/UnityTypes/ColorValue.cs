using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IColorValue : IValue<Color>
    {
    }

    [Serializable]
    [JungleClassInfo("Color Value", "Stores a Color value locally on the owner.", null, "Values/Unity Types", true)]
    public class ColorValue : LocalValue<Color>, IColorValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Color Member Value", "Returns a Color value from a component field, property, or method.", null, "Values/Unity Types")]
    public class ColorClassMembersValue : ClassMembersValue<Color>, IColorValue
    {
    }
}
