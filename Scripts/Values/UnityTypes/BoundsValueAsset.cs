using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    /// <summary>
    /// ScriptableObject storing a Bounds value for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Bounds value", fileName = "BoundsValue")]
    public class BoundsValueAsset : ValueAsset<Bounds>
    {
        [SerializeField]
        private Bounds value = new Bounds(Vector3.zero, Vector3.one);
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Bounds Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Bounds value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Bounds values for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Bounds list value", fileName = "BoundsListValue")]
    public class BoundsListValueAsset : SerializedValueListAsset<Bounds>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Bounds value from the assigned BoundsValueAsset.
    /// </summary>
    [Serializable]
    public class BoundsValueFromAsset : ValueFromAsset<Bounds, BoundsValueAsset>, IBoundsValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Bounds values from an assigned BoundsListValueAsset.
    /// </summary>
    [Serializable]
    public class BoundsListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Bounds>, BoundsListValueAsset>
    {
    }
}
