using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color32 Value", fileName = "Color32Value")]
    public class Color32ValueAsset : ValueAsset<Color32>, IColor32Value
    {
        [SerializeField]
        private Color32 value = new Color32(255, 255, 255, 255);

        public override Color32 GetValue()
        {
            return value;
        }
    }
}
