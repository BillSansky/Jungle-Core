using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Color32 value.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color32 value", fileName = "Color32Value")]
    [JungleClassInfo("Color32 Value Asset", "ScriptableObject storing a Color32 value.", null, "Values/Unity Types")]
    public class Color32ValueAsset : ValueAsset<Color32>
    {
        [SerializeField]
        private Color32 value = new Color32(255, 255, 255, 255);
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Color32 Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Color32 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of Color32 values.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color32 list value", fileName = "Color32ListValue")]
    [JungleClassInfo("Color32 List Asset", "ScriptableObject storing a list of Color32 values.", null, "Values/Unity Types")]
    public class Color32ListValueAsset : SerializedValueListAsset<Color32>
    {
    }
    /// <summary>
    /// Reads a Color32 value from a Color32ValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color32 Value From Asset", "Reads a Color32 value from a Color32ValueAsset.", null, "Values/Unity Types")]
    public class Color32ValueFromAsset : ValueFromAsset<Color32, Color32ValueAsset>, ISettableColor32Value
    {
    }
    /// <summary>
    /// Reads Color32 values from a Color32ListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Color32 List From Asset", "Reads Color32 values from a Color32ListValueAsset.", null, "Values/Unity Types")]
    public class Color32ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Color32>, Color32ListValueAsset>
    {
    }
}
