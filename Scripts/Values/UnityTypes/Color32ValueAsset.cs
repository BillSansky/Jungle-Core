using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color32 value", fileName = "Color32Value")]
    [JungleClassInfo("Color32 Value Asset", "ScriptableObject storing a Color32 value.", null, "Values/Unity Types")]
    public class Color32ValueAsset : ValueAsset<Color32>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color32 list value", fileName = "Color32ListValue")]
    [JungleClassInfo("Color32 List Asset", "ScriptableObject storing a list of Color32 values.", null, "Values/Unity Types")]
    public class Color32ListValueAsset : SerializedValueListAsset<Color32>
    {
    }

    [Serializable]
    [JungleClassInfo("Color32 Value From Asset", "Reads a Color32 value from a Color32ValueAsset.", null, "Values/Unity Types")]
    public class Color32ValueFromAsset : ValueFromAsset<Color32, Color32ValueAsset>, IColor32Value
    {
    }

    [Serializable]
    [JungleClassInfo("Color32 List From Asset", "Reads Color32 values from a Color32ListValueAsset.", null, "Values/Unity Types")]
    public class Color32ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Color32>, Color32ListValueAsset>
    {
    }
}
