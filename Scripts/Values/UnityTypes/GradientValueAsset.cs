using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Gradient value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Gradient value", fileName = "GradientValue")]
    public class GradientValueAsset : ValueAsset<Gradient>
    {
        [SerializeField]
        private Gradient value = new Gradient();
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Gradient Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Gradient value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Gradient values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Gradient list value", fileName = "GradientListValue")]
    public class GradientListValueAsset : SerializedValueListAsset<Gradient>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Gradient value from the assigned GradientValueAsset.
    /// </summary>
    [Serializable]
    public class GradientValueFromAsset : ValueFromAsset<Gradient, GradientValueAsset>, IGradientValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Gradient values from an assigned GradientListValueAsset.
    /// </summary>
    [Serializable]
    public class GradientListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Gradient>, GradientListValueAsset>
    {
    }
}
