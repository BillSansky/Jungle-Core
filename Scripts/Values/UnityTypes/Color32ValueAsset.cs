using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color32 value", fileName = "Color32Value")]
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
    public class Color32ListValueAsset : SerializedValueListAsset<Color32>
    {
    }

    [Serializable]
    public class Color32ValueFromAsset : ValueFromAsset<Color32, Color32ValueAsset>, IColor32Value
    {
    }

    [Serializable]
    public class Color32ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Color32>, Color32ListValueAsset>
    {
    }
}
