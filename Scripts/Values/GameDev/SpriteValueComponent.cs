using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class SpriteValueComponent : ValueComponent<Sprite>, ISpriteValue
    {
        [SerializeField]
        private Sprite value;

        public override Sprite GetValue()
        {
            return value;
        }
    }
}
