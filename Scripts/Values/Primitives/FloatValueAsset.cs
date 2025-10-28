using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float value", fileName = "FloatValue")]
    public class FloatValueAsset : ValueAsset<float>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float list value", fileName = "FloatListValue")]
    public class FloatListValueAsset : SerializedValueListAsset<float>
    {
    }

    [Serializable]
    public class FloatValueFromAsset : ValueFromAsset<float, FloatValueAsset>, IFloatValue
    {
    }

    [Serializable]
    public class FloatListValueFromAsset : ValueFromAsset<IReadOnlyList<float>, FloatListValueAsset>
    {
    }
}
