using System;
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
    }

    [Serializable]
    public class FloatValueFromComponent : ValueFromComponent<float, FloatValueComponent>, IFloatValue
    {
    }
}
