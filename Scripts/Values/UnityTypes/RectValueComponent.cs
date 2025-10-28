using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class RectValueComponent : ValueComponent<Rect>
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

    public class RectListValueComponent : SerializedValueListComponent<Rect>
    {
    }

    [Serializable]
    public class RectValueFromComponent : ValueFromComponent<Rect, RectValueComponent>, IRectValue
    {
    }

    [Serializable]
    public class RectListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Rect>, RectListValueComponent>
    {
    }
}
