using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color Value", fileName = "ColorValue")]
    public class ColorValueAsset : ValueAsset<Color>
    {
        [SerializeField]
        private Color value = Color.white;

        public override Color GetValue()
        {
            return value;
        }
    }
}
