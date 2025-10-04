using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Sprite value", fileName = "SpriteValue")]
    public class SpriteValueAsset : ValueAsset<Sprite>
    {
        [SerializeField]
        private Sprite value;

        public override Sprite Value()
        {
            return value;
        }
    }

    [Serializable]
    public class SpriteValueFromAsset : ValueFromAsset<Sprite, SpriteValueAsset>, ISpriteValue
    {
    }
}
