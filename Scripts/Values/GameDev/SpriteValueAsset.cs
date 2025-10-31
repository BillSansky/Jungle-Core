using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Sprite value", fileName = "SpriteValue")]
    [JungleClassInfo("Sprite Value Asset", "ScriptableObject storing a sprite asset.", null, "Values/Game Dev")]
    public class SpriteValueAsset : ValueAsset<Sprite>
    {
        [SerializeField]
        private Sprite value;

        public override Sprite Value()
        {
            return value;
        }

        public override void SetValue(Sprite value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Sprite list value", fileName = "SpriteListValue")]
    [JungleClassInfo("Sprite List Asset", "ScriptableObject storing a list of sprites.", null, "Values/Game Dev")]
    public class SpriteListValueAsset : SerializedValueListAsset<Sprite>
    {
    }

    [Serializable]
    [JungleClassInfo("Sprite Value From Asset", "Reads a sprite asset from a SpriteValueAsset.", null, "Values/Game Dev")]
    public class SpriteValueFromAsset : ValueFromAsset<Sprite, SpriteValueAsset>, ISpriteValue
    {
    }

    [Serializable]
    [JungleClassInfo("Sprite List From Asset", "Reads sprites from a SpriteListValueAsset.", null, "Values/Game Dev")]
    public class SpriteListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Sprite>, SpriteListValueAsset>
    {
    }
}
