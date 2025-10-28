using System;
using System.Collections.Generic;
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

    public class SpriteListValueComponent : SerializedValueListComponent<Sprite>
    {
    }

    [Serializable]
    public class SpriteValueFromComponent :
        ValueFromComponent<Sprite, SpriteValueComponent>, ISpriteValue
    {
    }

    [Serializable]
    public class SpriteListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Sprite>, SpriteListValueComponent>
    {
    }
}
