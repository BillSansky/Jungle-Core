using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double value", fileName = "DoubleValue")]
    public class DoubleValueAsset : ValueAsset<double>
    {
        [SerializeField]
        private double value;

        public override double Value()
        {
            return value;
        }

        public override void SetValue(double value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double list value", fileName = "DoubleListValue")]
    public class DoubleListValueAsset : SerializedValueListAsset<double>
    {
    }

    [Serializable]
    public class DoubleValueFromAsset : ValueFromAsset<double, DoubleValueAsset>, IDoubleValue
    {
    }

    [Serializable]
    public class DoubleListValueFromAsset : ValueFromAsset<IReadOnlyList<double>, DoubleListValueAsset>
    {
    }
}
