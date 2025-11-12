using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a sprite asset.
    /// </summary>
    [JungleClassInfo("Sprite Value Component", "Component exposing a sprite asset.", null, "Values/Game Dev")]
    public class SpriteValueComponent : ValueComponent<Sprite>
    {
        [SerializeField]
        private Sprite value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Sprite Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Sprite value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of sprites.
    /// </summary>

    [JungleClassInfo("Sprite List Component", "Component exposing a list of sprites.", null, "Values/Game Dev")]
    public class SpriteListValueComponent : SerializedValueListComponent<Sprite>
    {
    }
    /// <summary>
    /// Reads a sprite asset from a SpriteValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Sprite Value From Component", "Reads a sprite asset from a SpriteValueComponent.", null, "Values/Game Dev")]
    public class SpriteValueFromComponent :
        ValueFromComponent<Sprite, SpriteValueComponent>, ISettableSpriteValue
    {
    }
    /// <summary>
    /// Reads sprites from a SpriteListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Sprite List From Component", "Reads sprites from a SpriteListValueComponent.", null, "Values/Game Dev")]
    public class SpriteListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Sprite>, SpriteListValueComponent>
    {
    }
}
