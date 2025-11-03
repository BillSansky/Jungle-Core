using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a sprite asset.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Sprite value", fileName = "SpriteValue")]
    [JungleClassInfo("Sprite Value Asset", "ScriptableObject storing a sprite asset.", null, "Values/Game Dev")]
    public class SpriteValueAsset : ValueAsset<Sprite>
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
    /// ScriptableObject storing a list of sprites.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Sprite list value", fileName = "SpriteListValue")]
    [JungleClassInfo("Sprite List Asset", "ScriptableObject storing a list of sprites.", null, "Values/Game Dev")]
    public class SpriteListValueAsset : SerializedValueListAsset<Sprite>
    {
    }
    /// <summary>
    /// Reads a sprite asset from a SpriteValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Sprite Value From Asset", "Reads a sprite asset from a SpriteValueAsset.", null, "Values/Game Dev")]
    public class SpriteValueFromAsset : ValueFromAsset<Sprite, SpriteValueAsset>, ISpriteValue
    {
    }
    /// <summary>
    /// Reads sprites from a SpriteListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Sprite List From Asset", "Reads sprites from a SpriteListValueAsset.", null, "Values/Game Dev")]
    public class SpriteListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Sprite>, SpriteListValueAsset>
    {
    }
}
