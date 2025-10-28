using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class DoubleValueComponent : ValueComponent<double>
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

    public class DoubleListValueComponent : SerializedValueListComponent<double>
    {
    }

    [Serializable]
    public class DoubleValueFromComponent : ValueFromComponent<double, DoubleValueComponent>, IDoubleValue
    {
    }

    [Serializable]
    public class DoubleListValueFromComponent : ValueFromComponent<IReadOnlyList<double>, DoubleListValueComponent>
    {
    }
}
