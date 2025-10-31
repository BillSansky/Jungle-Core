using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Color Value Component", "Component exposing a Color value.", null, "Values/Unity Types")]
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

    [JungleClassInfo("Color List Component", "Component exposing a list of colors.", null, "Values/Unity Types")]
    public class ColorListValueComponent : SerializedValueListComponent<Color>
    {
    }

    [Serializable]
    [JungleClassInfo("Color Value From Component", "Reads a Color value from a ColorValueComponent.", null, "Values/Unity Types")]
    public class ColorValueFromComponent : ValueFromComponent<Color, ColorValueComponent>, IColorValue
    {
    }

    [Serializable]
    [JungleClassInfo("Color List From Component", "Reads colors from a ColorListValueComponent.", null, "Values/Unity Types")]
    public class ColorListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Color>, ColorListValueComponent>
    {
    }
}
