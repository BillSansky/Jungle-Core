using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a double-precision number.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double value", fileName = "DoubleValue")]
    [JungleClassInfo("Double Value Asset", "ScriptableObject storing a double-precision number.", null, "Values/Primitives")]
    public class DoubleValueAsset : ValueAsset<double>
    {
        [SerializeField]
        private double value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override double Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(double value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of double-precision numbers.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double list value", fileName = "DoubleListValue")]
    [JungleClassInfo("Double List Asset", "ScriptableObject storing a list of double-precision numbers.", null, "Values/Primitives")]
    public class DoubleListValueAsset : SerializedValueListAsset<double>
    {
    }
    /// <summary>
    /// Reads a double-precision number from a DoubleValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Double Value From Asset", "Reads a double-precision number from a DoubleValueAsset.", null, "Values/Primitives")]
    public class DoubleValueFromAsset : ValueFromAsset<double, DoubleValueAsset>, ISettableDoubleValue
    {
    }
    /// <summary>
    /// Reads double-precision numbers from a DoubleListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Double List From Asset", "Reads double-precision numbers from a DoubleListValueAsset.", null, "Values/Primitives")]
    public class DoubleListValueFromAsset : ValueFromAsset<IReadOnlyList<double>, DoubleListValueAsset>
    {
    }
}
