using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [Serializable]
    [JungleClassInfo("Collider Value Component", "Component exposing a collider component.", null, "Values/Game Dev")]
    public class ColliderValueComponent : ValueComponent<Collider>
    {
        [SerializeField]
        private Collider value;

        public override Collider Value()
        {
            return value;
        }

        public override void SetValue(Collider value)
        {
            this.value = value;
        }
    }

    [JungleClassInfo("Collider List Component", "Component exposing a list of colliders.", null, "Values/Game Dev")]
    public class ColliderListValueComponent : SerializedValueListComponent<Collider>
    {
    }

    [Serializable]
    [JungleClassInfo("Collider Value From Component", "Reads a collider component from a ColliderValueComponent.", null, "Values/Game Dev")]
    public class ColliderValueFromComponent :
        ValueFromComponent<Collider, ColliderValueComponent>, IColliderValue
    {
    }

    [Serializable]
    [JungleClassInfo("Collider List From Component", "Reads colliders from a ColliderListValueComponent.", null, "Values/Game Dev")]
    public class ColliderListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Collider>, ColliderListValueComponent>
    {
    }
}
