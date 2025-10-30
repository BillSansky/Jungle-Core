using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IColorValue : IValue<Color>
    {
    }

    [Serializable]
    public class ColorValue : LocalValue<Color>, IColorValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class ColorClassMembersValue : ClassMembersValue<Color>, IColorValue
    {
    }
}
