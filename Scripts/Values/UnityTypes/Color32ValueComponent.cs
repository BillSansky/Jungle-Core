using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Color32 Value Component", "Component exposing a Color32 value.", null, "Values/Unity Types")]
    public class Color32ValueComponent : ValueComponent<Color32>
    {
        [SerializeField]
        private Color32 value = new Color32(255, 255, 255, 255);

        public override Color32 Value()
        {
            return value;
        }

        public override void SetValue(Color32 value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Color32 List Component", "Component exposing a list of Color32 values.", null, "Values/Unity Types")]
    public class Color32ListValueComponent : SerializedValueListComponent<Color32>
    {
    }

    [Serializable]
    [JungleClassInfo("Color32 Value From Component", "Reads a Color32 value from a Color32ValueComponent.", null, "Values/Unity Types")]
    public class Color32ValueFromComponent : ValueFromComponent<Color32, Color32ValueComponent>, IColor32Value
    {
    }

    [Serializable]
    [JungleClassInfo("Color32 List From Component", "Reads Color32 values from a Color32ListValueComponent.", null, "Values/Unity Types")]
    public class Color32ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Color32>, Color32ListValueComponent>
    {
    }
}
