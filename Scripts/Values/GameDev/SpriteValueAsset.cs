using System;
using System.Collections.Generic;
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

        public override void SetValue(Sprite value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Sprite list value", fileName = "SpriteListValue")]
    public class SpriteListValueAsset : SerializedValueListAsset<Sprite>
    {
    }

    [Serializable]
    public class SpriteValueFromAsset : ValueFromAsset<Sprite, SpriteValueAsset>, ISpriteValue
    {
    }

    [Serializable]
    public class SpriteListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Sprite>, SpriteListValueAsset>
    {
    }
}
