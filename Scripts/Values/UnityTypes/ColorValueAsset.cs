using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a color value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color value", fileName = "ColorValue")]
    public class ColorValueAsset : ValueAsset<Color>
    {
        [SerializeField]
        private Color value = Color.white;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Color Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Color value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of color values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color list value", fileName = "ColorListValue")]
    public class ColorListValueAsset : SerializedValueListAsset<Color>
    {
    }
    /// <summary>
    /// Value wrapper that reads a color value from the assigned ColorValueAsset.
    /// </summary>
    [Serializable]
    public class ColorValueFromAsset : ValueFromAsset<Color, ColorValueAsset>, IColorValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of color values from an assigned ColorListValueAsset.
    /// </summary>
    [Serializable]
    public class ColorListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Color>, ColorListValueAsset>
    {
    }
}
