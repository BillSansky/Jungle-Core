using System;
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
    }

    [Serializable]
    public class ColliderValueFromComponent :
        ValueFromComponent<Collider, ColliderValueComponent>, IColliderValue
    {
    }
}
