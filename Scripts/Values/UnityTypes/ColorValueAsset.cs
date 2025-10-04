using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color value", fileName = "ColorValue")]
    public class ColorValueAsset : ValueAsset<Color>
    {
        [SerializeField]
        private Color value = Color.white;

        public override Color Value()
        {
            return value;
        }
    }

    [Serializable]
    public class ColorValueFromAsset : ValueFromAsset<Color, ColorValueAsset>, IColorValue
    {
    }
}
