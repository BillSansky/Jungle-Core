using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a Collider reference for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collider value", fileName = "ColliderLocalValue")]
    public class ColliderValueAsset : ValueAsset<Collider>
    {
        [SerializeField]
        private Collider value;
        /// <summary>
        /// Returns the serialized value stored in the asset.
        /// </summary>
        public override Collider Value()
        {
            return value;
        }
        /// <summary>
        /// Replaces the serialized value stored in the asset.
        /// </summary>
        public override void SetValue(Collider value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a reusable list of Collider references for Jungle value bindings.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collider list value", fileName = "ColliderListValue")]
    public class ColliderListValueAsset : SerializedValueListAsset<Collider>
    {
    }
    /// <summary>
    /// Value wrapper that reads a Collider reference from an assigned ColliderValueAsset.
    /// </summary>
    [Serializable]
    public class ColliderValueFromAsset :
        ValueFromAsset<Collider, ColliderValueAsset>, IColliderValue
    {
    }
    /// <summary>
    /// Value wrapper that reads a list of Collider references from an assigned ColliderListValueAsset.
    /// </summary>
    [Serializable]
    public class ColliderListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Collider>, ColliderListValueAsset>
    {
    }
}
