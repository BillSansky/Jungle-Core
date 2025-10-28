using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class ColorValueComponent : ValueComponent<Color>
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

    public class ColorListValueComponent : SerializedValueListComponent<Color>
    {
    }

    [Serializable]
    public class ColorValueFromComponent : ValueFromComponent<Color, ColorValueComponent>, IColorValue
    {
    }

    [Serializable]
    public class ColorListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Color>, ColorListValueComponent>
    {
    }
}
