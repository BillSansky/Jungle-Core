using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [JungleClassInfo("Sprite Value Component", "Component exposing a sprite asset.", null, "Values/Game Dev")]
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

    [JungleClassInfo("Sprite List Component", "Component exposing a list of sprites.", null, "Values/Game Dev")]
    public class SpriteListValueComponent : SerializedValueListComponent<Sprite>
    {
    }

    [Serializable]
    [JungleClassInfo("Sprite Value From Component", "Reads a sprite asset from a SpriteValueComponent.", null, "Values/Game Dev")]
    public class SpriteValueFromComponent :
        ValueFromComponent<Sprite, SpriteValueComponent>, ISpriteValue
    {
    }

    [Serializable]
    [JungleClassInfo("Sprite List From Component", "Reads sprites from a SpriteListValueComponent.", null, "Values/Game Dev")]
    public class SpriteListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Sprite>, SpriteListValueComponent>
    {
    }
}
