using System;
using System.Collections.Generic;
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

        public override void SetValue(Rect value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Rect list value", fileName = "RectListValue")]
    public class RectListValueAsset : SerializedValueListAsset<Rect>
    {
    }

    [Serializable]
    public class RectValueFromAsset : ValueFromAsset<Rect, RectValueAsset>, IRectValue
    {
    }

    [Serializable]
    public class RectListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Rect>, RectListValueAsset>
    {
    }
}
