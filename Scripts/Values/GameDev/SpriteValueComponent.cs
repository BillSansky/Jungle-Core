using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class SpriteValueComponent : ValueComponent<Sprite>
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

    [Serializable]
    public class SpriteValueFromComponent :
        ValueFromComponent<Sprite, SpriteValueComponent>, ISpriteValue
    {
    }
}
