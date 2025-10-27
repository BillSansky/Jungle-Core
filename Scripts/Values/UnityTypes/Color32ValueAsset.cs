using System;
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

    [Serializable]
    public class Color32ValueFromAsset : ValueFromAsset<Color32, Color32ValueAsset>, IColor32Value
    {
    }
}
