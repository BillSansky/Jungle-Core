using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IBoundsValue : IValue<Bounds>
    {
    }

    [Serializable]
    public class BoundsValue : LocalValue<Bounds>, IBoundsValue
    {
        public override bool HasMultipleValues => false;
        
    }
}
