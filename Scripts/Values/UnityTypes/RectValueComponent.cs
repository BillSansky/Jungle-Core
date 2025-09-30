using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class RectValueComponent : ValueComponent<Rect>
    {
        [SerializeField]
        private Rect value = new Rect(0f, 0f, 1f, 1f);

        public override Rect GetValue()
        {
            return value;
        }
    }
}
