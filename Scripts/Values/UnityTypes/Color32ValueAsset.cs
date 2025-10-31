using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Color32 value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color32 value", fileName = "Color32Value")]
    public class Color32ValueAsset : ValueAsset<Color32>
    {
        [SerializeField]
        private Color32 value = new Color32(255, 255, 255, 255);
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Color32 Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Color32 value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Color32 values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Color32 list value", fileName = "Color32ListValue")]
    public class Color32ListValueAsset : SerializedValueListAsset<Color32>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Color32 value from the assigned Color32ValueAsset.
    /// </summary>
    [Serializable]
    public class Color32ValueFromAsset : ValueFromAsset<Color32, Color32ValueAsset>, IColor32Value
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Color32 values from an assigned Color32ListValueAsset.
    /// </summary>
    [Serializable]
    public class Color32ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Color32>, Color32ListValueAsset>
    {
    }
}
