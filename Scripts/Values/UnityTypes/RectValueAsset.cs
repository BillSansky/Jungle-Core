using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Rect value", fileName = "RectValue")]
    public class RectValueAsset : ValueAsset<Rect>
    {
        [SerializeField]
        private Rect value = new Rect(0f, 0f, 1f, 1f);

        public override Rect Value()
        {
            return value;
        }
    }

    [Serializable]
    public class RectValueFromAsset : ValueFromAsset<Rect, RectValueAsset>, IRectValue
    {
    }
}
