using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public interface IColor32Value : IValue<Color32>
    {
    }

    [Serializable]
    public class Color32Value : LocalValue<Color32>, IColor32Value
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class Color32ClassMembersValue : ClassMembersValue<Color32>, IColor32Value
    {
    }
}
