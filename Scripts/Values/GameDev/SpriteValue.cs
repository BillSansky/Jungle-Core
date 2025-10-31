using System;
using Jungle.Values;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Defines the ISpriteValue contract.
    /// </summary>
    public interface ISpriteValue : IValue<Sprite>
    {
    }
    /// <summary>
    /// Stores a Sprite reference directly on the owning object for Jungle value bindings.
    /// </summary>
    [Serializable]
    public class SpriteValue : LocalValue<Sprite>, ISpriteValue
    {
        public override bool HasMultipleValues => false;
        
    }
    /// <summary>
    /// Resolves a Sprite reference by invoking the selected member on a component.
    /// </summary>
    [Serializable]
    public class SpriteClassMembersValue : ClassMembersValue<Sprite>, ISpriteValue
    {
    }
}
