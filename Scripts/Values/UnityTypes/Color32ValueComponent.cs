using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Color32ValueComponent : ValueComponent<Color32>
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

    public class Color32ListValueComponent : SerializedValueListComponent<Color32>
    {
    }

    [Serializable]
    public class Color32ValueFromComponent : ValueFromComponent<Color32, Color32ValueComponent>, IColor32Value
    {
    }

    [Serializable]
    public class Color32ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Color32>, Color32ListValueComponent>
    {
    }
}
