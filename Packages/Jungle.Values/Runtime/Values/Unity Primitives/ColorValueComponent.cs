using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// Component exposing a Color value.
    /// </summary>
    [JungleClassInfo("Color Value Component", "Component exposing a Color value.", null, "Values/Unity Primitives")]
    public class ColorValueComponent : ValueComponent<Color>
    {
        [SerializeField]
        private Color value = Color.white;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Color Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Color value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// Component exposing a list of colors.
    /// </summary>

    [JungleClassInfo("Color List Component", "Component exposing a list of colors.", null, "Values/Unity Primitives")]
    public class ColorListValueComponent : SerializedValueListComponent<Color>
    {
    }
    /// <summary>
    /// Reads a Color value from a ColorValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color Value From Component", "Reads a Color value from a ColorValueComponent.", null, "Values/Unity Primitives")]
    public class ColorValueFromComponent : ValueFromComponent<Color, ColorValueComponent>, ISettableColorValue
    {
    }
    /// <summary>
    /// Reads colors from a ColorListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color List From Component", "Reads colors from a ColorListValueComponent.", null, "Values/Unity Primitives")]
    public class ColorListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Color>, ColorListValueComponent>
    {
    }
}
