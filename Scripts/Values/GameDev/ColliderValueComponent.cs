using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [Serializable]
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

    public class ColliderListValueComponent : SerializedValueListComponent<Collider>
    {
    }

    [Serializable]
    public class ColliderValueFromComponent :
        ValueFromComponent<Collider, ColliderValueComponent>, IColliderValue
    {
    }

    [Serializable]
    public class ColliderListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Collider>, ColliderListValueComponent>
    {
    }
}
