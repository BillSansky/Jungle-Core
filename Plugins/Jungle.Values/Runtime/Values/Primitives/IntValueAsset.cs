using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing a integer number.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int value", fileName = "IntValue")]
    [JungleClassInfo("Int Value Asset", "ScriptableObject storing a integer number.", null, "Values/Primitives")]
    public class IntValueAsset : ValueAsset<int>
    {
        [SerializeField]
        private int value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override int Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of integer numbers.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int list value", fileName = "IntListValue")]
    [JungleClassInfo("Int List Asset", "ScriptableObject storing a list of integer numbers.", null, "Values/Primitives")]
    public class IntListValueAsset : SerializedValueListAsset<int>
    {
    }
    /// <summary>
    /// Reads a integer number from a IntValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Int Value From Asset", "Reads a integer number from a IntValueAsset.", null, "Values/Primitives")]
    public class IntValueFromAsset : ValueFromAsset<int, IntValueAsset>, IIntValue
    {
    }
    /// <summary>
    /// Reads integer numbers from a IntListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Int List From Asset", "Reads integer numbers from a IntListValueAsset.", null, "Values/Primitives")]
    public class IntListValueFromAsset : ValueFromAsset<IReadOnlyList<int>, IntListValueAsset>
    {
    }
}
