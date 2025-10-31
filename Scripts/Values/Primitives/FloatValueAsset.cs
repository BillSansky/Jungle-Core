using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a float value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float value", fileName = "FloatValue")]
    public class FloatValueAsset : ValueAsset<float>
    {
        [SerializeField]
        private float value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override float Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(float value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of float values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float list value", fileName = "FloatListValue")]
    public class FloatListValueAsset : SerializedValueListAsset<float>
    {
    }
    /// <summary>
    /// Value wrapper that reads a float value from the assigned FloatValueAsset.
    /// </summary>
    [Serializable]
    public class FloatValueFromAsset : ValueFromAsset<float, FloatValueAsset>, IFloatValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of float values from the assigned FloatListValueAsset.
    /// </summary>
    [Serializable]
    public class FloatListValueFromAsset : ValueFromAsset<IReadOnlyList<float>, FloatListValueAsset>
    {
    }
}
