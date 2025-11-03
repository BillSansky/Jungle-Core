using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// Component exposing a floating-point number.
    /// </summary>
    [JungleClassInfo("Float Value Component", "Component exposing a floating-point number.", null, "Values/Primitives")]
    public class FloatValueComponent : ValueComponent<float>
    {
        [SerializeField]
        private float value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override float Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(float value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of floating-point numbers.
    /// </summary>

    [JungleClassInfo("Float List Component", "Component exposing a list of floating-point numbers.", null, "Values/Primitives")]
    public class FloatListValueComponent : SerializedValueListComponent<float>
    {
    }
    /// <summary>
    /// Reads a floating-point number from a FloatValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Float Value From Component", "Reads a floating-point number from a FloatValueComponent.", null, "Values/Primitives")]
    public class FloatValueFromComponent : ValueFromComponent<float, FloatValueComponent>, IFloatValue
    {
    }
    /// <summary>
    /// Reads floating-point numbers from a FloatListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Float List From Component", "Reads floating-point numbers from a FloatListValueComponent.", null, "Values/Primitives")]
    public class FloatListValueFromComponent : ValueFromComponent<IReadOnlyList<float>, FloatListValueComponent>
    {
    }
}
