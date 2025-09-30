using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class ColorValueComponent : ValueComponent<Color>
    {
        [SerializeField]
        private Color value = Color.white;

        public override Color GetValue()
        {
            return value;
        }
    }
}
