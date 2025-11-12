using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Component exposing a double-precision number.
    /// </summary>
    [JungleClassInfo("Double Value Component", "Component exposing a double-precision number.", null, "Primitives")]
    public class DoubleValueComponent : ValueComponent<double>
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
    /// Component exposing a list of double-precision numbers.
    /// </summary>

    [JungleClassInfo("Double List Component", "Component exposing a list of double-precision numbers.", null, "Primitives")]
    public class DoubleListValueComponent : SerializedValueListComponent<double>
    {
    }
    /// <summary>
    /// Reads a double-precision number from a DoubleValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Double Value From Component", "Reads a double-precision number from a DoubleValueComponent.", null, "Primitives")]
    public class DoubleValueFromComponent : ValueFromComponent<double, DoubleValueComponent>, ISettableDoubleValue
    {
    }
    /// <summary>
    /// Reads double-precision numbers from a DoubleListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Double List From Component", "Reads double-precision numbers from a DoubleListValueComponent.", null, "Primitives")]
    public class DoubleListValueFromComponent : ValueFromComponent<IReadOnlyList<double>, DoubleListValueComponent>
    {
    }
}
