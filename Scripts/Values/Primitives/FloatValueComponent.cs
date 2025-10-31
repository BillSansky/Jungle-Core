using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [JungleClassInfo("Float Value Component", "Component exposing a floating-point number.", null, "Values/Primitives")]
    public class FloatValueComponent : ValueComponent<float>
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

    [JungleClassInfo("Float List Component", "Component exposing a list of floating-point numbers.", null, "Values/Primitives")]
    public class FloatListValueComponent : SerializedValueListComponent<float>
    {
    }

    [Serializable]
    [JungleClassInfo("Float Value From Component", "Reads a floating-point number from a FloatValueComponent.", null, "Values/Primitives")]
    public class FloatValueFromComponent : ValueFromComponent<float, FloatValueComponent>, IFloatValue
    {
    }

    [Serializable]
    [JungleClassInfo("Float List From Component", "Reads floating-point numbers from a FloatListValueComponent.", null, "Values/Primitives")]
    public class FloatListValueFromComponent : ValueFromComponent<IReadOnlyList<float>, FloatListValueComponent>
    {
    }
}
