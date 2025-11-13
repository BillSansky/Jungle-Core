using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a boolean flag.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Bool value", fileName = "BoolValue")]
    [JungleClassInfo("Bool Value Asset", "ScriptableObject storing a boolean flag.", null, "Values/Primitives")]
    public class BoolValueAsset : ValueAsset<bool>
    {
        [SerializeField]
        private bool value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override bool Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(bool value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of boolean flags.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Bool list value", fileName = "BoolListValue")]
    [JungleClassInfo("Bool List Asset", "ScriptableObject storing a list of boolean flags.", null, "Values/Primitives")]
    public class BoolListValueAsset : SerializedValueListAsset<bool>
    {
    }
    /// <summary>
    /// Reads a boolean flag from a BoolValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bool Value From Asset", "Reads a boolean flag from a BoolValueAsset.", null, "Values/Primitives")]
    public class BoolValueFromAsset : ValueFromAsset<bool, BoolValueAsset>, ISettableBoolValue
    {
    }
    /// <summary>
    /// Reads boolean flags from a BoolListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Bool List From Asset", "Reads boolean flags from a BoolListValueAsset.", null, "Values/Primitives")]
    public class BoolListValueFromAsset : ValueFromAsset<IReadOnlyList<bool>, BoolListValueAsset>
    {
    }
}
