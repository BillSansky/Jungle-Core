using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Color value.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color value", fileName = "ColorValue")]
    [JungleClassInfo("Color Value Asset", "ScriptableObject storing a Color value.", null, "Values/Unity Types")]
    public class ColorValueAsset : ValueAsset<Color>
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
    /// ScriptableObject storing a list of colors.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color list value", fileName = "ColorListValue")]
    [JungleClassInfo("Color List Asset", "ScriptableObject storing a list of colors.", null, "Values/Unity Types")]
    public class ColorListValueAsset : SerializedValueListAsset<Color>
    {
    }
    /// <summary>
    /// Reads a Color value from a ColorValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color Value From Asset", "Reads a Color value from a ColorValueAsset.", null, "Values/Unity Types")]
    public class ColorValueFromAsset : ValueFromAsset<Color, ColorValueAsset>, IColorValue
    {
    }
    /// <summary>
    /// Reads colors from a ColorListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color List From Asset", "Reads colors from a ColorListValueAsset.", null, "Values/Unity Types")]
    public class ColorListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Color>, ColorListValueAsset>
    {
    }
}
