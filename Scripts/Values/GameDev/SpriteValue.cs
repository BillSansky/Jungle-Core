using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public interface ISpriteValue : IValue<Sprite>
    {
    }

    [Serializable]
    public class SpriteValue : LocalValue<Sprite>, ISpriteValue
    {
    }
}
