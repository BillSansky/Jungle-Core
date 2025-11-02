using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    /// <summary>
    /// Component exposing a collider component.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Collider Value Component", "Component exposing a collider component.", null, "Values/Game Dev")]
    public class ColliderValueComponent : ValueComponent<Collider>
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
    /// Component exposing a list of colliders.
    /// </summary>

    [JungleClassInfo("Collider List Component", "Component exposing a list of colliders.", null, "Values/Game Dev")]
    public class ColliderListValueComponent : SerializedValueListComponent<Collider>
    {
    }
    /// <summary>
    /// Reads a collider component from a ColliderValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Collider Value From Component", "Reads a collider component from a ColliderValueComponent.", null, "Values/Game Dev")]
    public class ColliderValueFromComponent :
        ValueFromComponent<Collider, ColliderValueComponent>, IColliderValue
    {
    }
    /// <summary>
    /// Reads colliders from a ColliderListValueComponent.
    /// </summary>

    [Serializable]
    [JungleClassInfo("Collider List From Component", "Reads colliders from a ColliderListValueComponent.", null, "Values/Game Dev")]
    public class ColliderListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Collider>, ColliderListValueComponent>
    {
    }
}
