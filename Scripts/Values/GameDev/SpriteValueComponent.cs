using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class SpriteValueComponent : ValueComponent<Sprite>
    {
        [SerializeField]
        private Sprite value;

        public override Sprite GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class SpriteValueFromComponent :
        ValueFromComponent<Sprite, SpriteValueComponent>, ISpriteValue
    {
    }
}
