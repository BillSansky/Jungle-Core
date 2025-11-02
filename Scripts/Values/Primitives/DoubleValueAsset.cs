using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double value", fileName = "DoubleValue")]
    [JungleClassInfo("Double Value Asset", "ScriptableObject storing a double-precision number.", null, "Values/Primitives")]
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
    [JungleClassInfo("Double List Asset", "ScriptableObject storing a list of double-precision numbers.", null, "Values/Primitives")]
    public class DoubleListValueAsset : SerializedValueListAsset<double>
    {
    }

    [Serializable]
    [JungleClassInfo("Double Value From Asset", "Reads a double-precision number from a DoubleValueAsset.", null, "Values/Primitives")]
    public class DoubleValueFromAsset : ValueFromAsset<double, DoubleValueAsset>, IDoubleValue
    {
    }

    [Serializable]
    [JungleClassInfo("Double List From Asset", "Reads double-precision numbers from a DoubleListValueAsset.", null, "Values/Primitives")]
    public class DoubleListValueFromAsset : ValueFromAsset<IReadOnlyList<double>, DoubleListValueAsset>
    {
    }
}
