using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a Rigidbody reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Rigidbody value", fileName = "RigidbodyValue")]
    public class RigidbodyValueAsset : ValueAsset<Rigidbody>
    {
        [SerializeField]
        private Rigidbody value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Rigidbody Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Rigidbody value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Rigidbody references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Rigidbody list value", fileName = "RigidbodyListValue")]
    public class RigidbodyListValueAsset : SerializedValueListAsset<Rigidbody>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Rigidbody reference from an assigned RigidbodyValueAsset.
    /// </summary>
    [Serializable]
    public class RigidbodyValueFromAsset :
        ValueFromAsset<Rigidbody, RigidbodyValueAsset>, IRigidbodyValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Rigidbody references from an assigned RigidbodyListValueAsset.
    /// </summary>
    [Serializable]
    public class RigidbodyListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Rigidbody>, RigidbodyListValueAsset>
    {
    }
}
