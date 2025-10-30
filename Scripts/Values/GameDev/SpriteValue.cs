using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ISpriteValue : IValue<Sprite>
    {
    }

    [Serializable]
    public class SpriteValue : LocalValue<Sprite>, ISpriteValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    public class SpriteClassMembersValue : ClassMembersValue<Sprite>, ISpriteValue
    {
    }
}
