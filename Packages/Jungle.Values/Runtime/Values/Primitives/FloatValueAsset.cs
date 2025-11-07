using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a floating-point number.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float value", fileName = "FloatValue")]
    [JungleClassInfo("Float Value Asset", "ScriptableObject storing a floating-point number.", null, "Values/Primitives")]
    public class FloatValueAsset : ValueAsset<float>
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
    /// ScriptableObject storing a list of floating-point numbers.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Float list value", fileName = "FloatListValue")]
    [JungleClassInfo("Float List Asset", "ScriptableObject storing a list of floating-point numbers.", null, "Values/Primitives")]
    public class FloatListValueAsset : SerializedValueListAsset<float>
    {
    }
    /// <summary>
    /// Reads a floating-point number from a FloatValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Float Value From Asset", "Reads a floating-point number from a FloatValueAsset.", null, "Values/Primitives")]
    public class FloatValueFromAsset : ValueFromAsset<float, FloatValueAsset>, IFloatValue
    {
    }
    /// <summary>
    /// Reads floating-point numbers from a FloatListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Float List From Asset", "Reads floating-point numbers from a FloatListValueAsset.", null, "Values/Primitives")]
    public class FloatListValueFromAsset : ValueFromAsset<IReadOnlyList<float>, FloatListValueAsset>
    {
    }
}
