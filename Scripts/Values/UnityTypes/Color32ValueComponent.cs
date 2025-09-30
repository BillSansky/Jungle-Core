using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Color32ValueComponent : ValueComponent<Color32>
    {
        [SerializeField]
        private Color32 value = new Color32(255, 255, 255, 255);

        public override Color32 GetValue()
        {
            return value;
        }
    }
}
