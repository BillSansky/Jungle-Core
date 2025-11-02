using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Rect value", fileName = "RectValue")]
    [JungleClassInfo("Rect Value Asset", "ScriptableObject storing a Rect area.", null, "Values/Unity Types")]
    public class RectValueAsset : ValueAsset<Rect>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Rect list value", fileName = "RectListValue")]
    [JungleClassInfo("Rect List Asset", "ScriptableObject storing a list of rectangles.", null, "Values/Unity Types")]
    public class RectListValueAsset : SerializedValueListAsset<Rect>
    {
    }

    [Serializable]
    [JungleClassInfo("Rect Value From Asset", "Reads a Rect area from a RectValueAsset.", null, "Values/Unity Types")]
    public class RectValueFromAsset : ValueFromAsset<Rect, RectValueAsset>, IRectValue
    {
    }

    [Serializable]
    [JungleClassInfo("Rect List From Asset", "Reads rectangles from a RectListValueAsset.", null, "Values/Unity Types")]
    public class RectListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Rect>, RectListValueAsset>
    {
    }
}
