using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a gradient.
    /// </summary>
    [JungleClassInfo("Gradient Value Component", "Component exposing a gradient.", null, "Unity Types")]
    public class GradientValueComponent : ValueComponent<Gradient>
    {
        [SerializeField]
        private Gradient value = new Gradient();
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Gradient Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Gradient value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of gradients.
    /// </summary>

    [JungleClassInfo("Gradient List Component", "Component exposing a list of gradients.", null, "Unity Types")]
    public class GradientListValueComponent : SerializedValueListComponent<Gradient>
    {
    }
    /// <summary>
    /// Reads a gradient from a GradientValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Gradient Value From Component", "Reads a gradient from a GradientValueComponent.", null, "Unity Types")]
    public class GradientValueFromComponent : ValueFromComponent<Gradient, GradientValueComponent>, ISettableGradientValue
    {
    }
    /// <summary>
    /// Reads gradients from a GradientListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Gradient List From Component", "Reads gradients from a GradientListValueComponent.", null, "Unity Types")]
    public class GradientListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Gradient>, GradientListValueComponent>
    {
    }
}
