using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Rect Value Component", "Component exposing a Rect area.", null, "Values/Unity Types")]
    public class RectValueComponent : ValueComponent<Rect>
    {
        [SerializeField]
        private Rect value = new Rect(0f, 0f, 1f, 1f);

        public override Rect Value()
        {
            return value;
        }

        public override void SetValue(Rect value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Rect List Component", "Component exposing a list of rectangles.", null, "Values/Unity Types")]
    public class RectListValueComponent : SerializedValueListComponent<Rect>
    {
    }

    [Serializable]
    [JungleClassInfo("Rect Value From Component", "Reads a Rect area from a RectValueComponent.", null, "Values/Unity Types")]
    public class RectValueFromComponent : ValueFromComponent<Rect, RectValueComponent>, IRectValue
    {
    }

    [Serializable]
    [JungleClassInfo("Rect List From Component", "Reads rectangles from a RectListValueComponent.", null, "Values/Unity Types")]
    public class RectListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Rect>, RectListValueComponent>
    {
    }
}
