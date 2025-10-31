using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a double value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double value", fileName = "DoubleValue")]
    public class DoubleValueAsset : ValueAsset<double>
    {
        [SerializeField]
        private double value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override double Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(double value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of double values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double list value", fileName = "DoubleListValue")]
    public class DoubleListValueAsset : SerializedValueListAsset<double>
    {
    }
    /// <summary>
    /// Value wrapper that reads a double value from the assigned DoubleValueAsset.
    /// </summary>
    [Serializable]
    public class DoubleValueFromAsset : ValueFromAsset<double, DoubleValueAsset>, IDoubleValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of double values from the assigned DoubleListValueAsset.
    /// </summary>
    [Serializable]
    public class DoubleListValueFromAsset : ValueFromAsset<IReadOnlyList<double>, DoubleListValueAsset>
    {
    }
}
