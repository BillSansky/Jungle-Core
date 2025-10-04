using System;
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
}
