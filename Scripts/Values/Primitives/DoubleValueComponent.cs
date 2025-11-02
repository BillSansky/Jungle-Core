using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [JungleClassInfo("Double Value Component", "Component exposing a double-precision number.", null, "Values/Primitives")]
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

    [JungleClassInfo("Double List Component", "Component exposing a list of double-precision numbers.", null, "Values/Primitives")]
    public class DoubleListValueComponent : SerializedValueListComponent<double>
    {
    }

    [Serializable]
    [JungleClassInfo("Double Value From Component", "Reads a double-precision number from a DoubleValueComponent.", null, "Values/Primitives")]
    public class DoubleValueFromComponent : ValueFromComponent<double, DoubleValueComponent>, IDoubleValue
    {
    }

    [Serializable]
    [JungleClassInfo("Double List From Component", "Reads double-precision numbers from a DoubleListValueComponent.", null, "Values/Primitives")]
    public class DoubleListValueFromComponent : ValueFromComponent<IReadOnlyList<double>, DoubleListValueComponent>
    {
    }
}
