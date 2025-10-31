using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    /// <summary>
    /// ScriptableObject storing an int value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int value", fileName = "IntValue")]
    public class IntValueAsset : ValueAsset<int>
    {
        [SerializeField]
        private int value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override int Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(int value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of int values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int list value", fileName = "IntListValue")]
    public class IntListValueAsset : SerializedValueListAsset<int>
    {
    }
    /// <summary>
    /// Value wrapper that reads an int value from the assigned IntValueAsset.
    /// </summary>
    [Serializable]
    public class IntValueFromAsset : ValueFromAsset<int, IntValueAsset>, IIntValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of int values from the assigned IntListValueAsset.
    /// </summary>
    [Serializable]
    public class IntListValueFromAsset : ValueFromAsset<IReadOnlyList<int>, IntListValueAsset>
    {
    }
}
