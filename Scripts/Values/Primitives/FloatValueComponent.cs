using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class FloatValueComponent : ValueComponent<float>
    {
        [SerializeField]
        private float value;

        public override float Value()
        {
            return value;
        }

        public override void SetValue(float value)
        {
            this.value = value;
        }
    }

    public class FloatListValueComponent : SerializedValueListComponent<float>
    {
    }

    [Serializable]
    public class FloatValueFromComponent : ValueFromComponent<float, FloatValueComponent>, IFloatValue
    {
    }

    [Serializable]
    public class FloatListValueFromComponent : ValueFromComponent<IReadOnlyList<float>, FloatListValueComponent>
    {
    }
}
