using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Color32ValueComponent : ValueComponent<Color32>
    {
        [SerializeField]
        private Color32 value = new Color32(255, 255, 255, 255);

        public override Color32 Value()
        {
            return value;
        }
    }

    [Serializable]
    public class Color32ValueFromComponent : ValueFromComponent<Color32, Color32ValueComponent>, IColor32Value
    {
    }
}
