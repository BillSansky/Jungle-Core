using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class DoubleValueComponent : ValueComponent<double>
    {
        [SerializeField]
        private double value;

        public override double GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class DoubleValueFromComponent : ValueFromComponent<double, DoubleValueComponent>, IDoubleValue
    {
    }
}
