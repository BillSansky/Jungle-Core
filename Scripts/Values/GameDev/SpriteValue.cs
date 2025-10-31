using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ISpriteValue : IValue<Sprite>
    {
    }

    [Serializable]
    [JungleClassInfo("Sprite Value", "Stores a sprite asset directly on the owner.", null, "Values/Game Dev", true)]
    public class SpriteValue : LocalValue<Sprite>, ISpriteValue
    {
        public override bool HasMultipleValues => false;
        
    }

    [Serializable]
    [JungleClassInfo("Sprite Member Value", "Returns a sprite asset from a component field, property, or method.", null, "Values/Game Dev")]
    public class SpriteClassMembersValue : ClassMembersValue<Sprite>, ISpriteValue
    {
    }
}
