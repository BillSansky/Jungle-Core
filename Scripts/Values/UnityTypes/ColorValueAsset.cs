using System;
using System.Collections.Generic;
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

        public override void SetValue(Color value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color list value", fileName = "ColorListValue")]
    public class ColorListValueAsset : SerializedValueListAsset<Color>
    {
    }

    [Serializable]
    public class ColorValueFromAsset : ValueFromAsset<Color, ColorValueAsset>, IColorValue
    {
    }

    [Serializable]
    public class ColorListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Color>, ColorListValueAsset>
    {
    }
}
