using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [JungleClassInfo("Gradient Value Component", "Component exposing a gradient.", null, "Values/Unity Types")]
    public class GradientValueComponent : ValueComponent<Gradient>
    {
        [SerializeField]
        private Gradient value = new Gradient();

        public override Gradient Value()
        {
            return value;
        }

        public override void SetValue(Gradient value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Gradient List Component", "Component exposing a list of gradients.", null, "Values/Unity Types")]
    public class GradientListValueComponent : SerializedValueListComponent<Gradient>
    {
    }

    [Serializable]
    [JungleClassInfo("Gradient Value From Component", "Reads a gradient from a GradientValueComponent.", null, "Values/Unity Types")]
    public class GradientValueFromComponent : ValueFromComponent<Gradient, GradientValueComponent>, IGradientValue
    {
    }

    [Serializable]
    [JungleClassInfo("Gradient List From Component", "Reads gradients from a GradientListValueComponent.", null, "Values/Unity Types")]
    public class GradientListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Gradient>, GradientListValueComponent>
    {
    }
}
