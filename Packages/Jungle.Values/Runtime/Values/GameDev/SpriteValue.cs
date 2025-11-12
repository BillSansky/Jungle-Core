using System;
using Jungle.Attributes;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Represents a value provider that returns a Sprite reference.
    /// </summary>
    public interface ISpriteValue : IValue<Sprite>
    {
    }
    public interface ISettableSpriteValue : ISpriteValue, IValueSableValue<Sprite>
    {
    }
    /// <summary>
    /// Stores a sprite asset directly on the owner.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Sprite Value", "Stores a sprite asset directly on the owner.", null, "Game Dev", true)]
    public class SpriteValue : LocalValue<Sprite>, ISettableSpriteValue
    {
        /// <summary>
        /// Indicates whether multiple values are available.
        /// </summary>
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Returns a sprite asset from a component field, property, or method.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Sprite Member Value", "Returns a sprite asset from a component field, property, or method.", null, "Game Dev")]
    public class SpriteClassMembersValue : ClassMembersValue<Sprite>, ISpriteValue
    {
    }
}
