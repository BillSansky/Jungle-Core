using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// ScriptableObject storing a collider component.
    /// </summary>
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collider value", fileName = "ColliderLocalValue")]
    [JungleClassInfo("Collider Value Asset", "ScriptableObject storing a collider component.", null, "Values/Game Dev")]
    public class ColliderValueAsset : ValueAsset<Collider>
    {
        [SerializeField]
        private Collider value;
        /// <summary>
        /// Gets the value produced by this provider.
        /// </summary>

        public override Collider Value()
        {
            return value;
        }
        /// <summary>
        /// Assigns a new value to the provider.
        /// </summary>

        public override void SetValue(Collider value)
        {
            this.value = value;
        }
    }
    /// <summary>
    /// ScriptableObject storing a list of colliders.
    /// </summary>

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collider list value", fileName = "ColliderListValue")]
    [JungleClassInfo("Collider List Asset", "ScriptableObject storing a list of colliders.", null, "Values/Game Dev")]
    public class ColliderListValueAsset : SerializedValueListAsset<Collider>
    {
    }
    /// <summary>
    /// Reads a collider component from a ColliderValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Collider Value From Asset", "Reads a collider component from a ColliderValueAsset.", null, "Values/Game Dev")]
    public class ColliderValueFromAsset :
        ValueFromAsset<Collider, ColliderValueAsset>, IColliderValue
    {
    }
    /// <summary>
    /// Reads colliders from a ColliderListValueAsset.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Collider List From Asset", "Reads colliders from a ColliderListValueAsset.", null, "Values/Game Dev")]
    public class ColliderListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Collider>, ColliderListValueAsset>
    {
    }
}
