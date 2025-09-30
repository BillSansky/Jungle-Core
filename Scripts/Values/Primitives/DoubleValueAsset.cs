using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double Value", fileName = "DoubleValue")]
    public class DoubleValueAsset : ValueAsset<double>
    {
        [SerializeField]
        private double value;

        public override double GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class DoubleValueFromAsset : ValueFromAsset<double, DoubleValueAsset>, IDoubleValue
    {
    }
}
