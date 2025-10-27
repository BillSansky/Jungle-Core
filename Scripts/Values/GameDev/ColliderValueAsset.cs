using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collider value", fileName = "ColliderLocalValue")]
    public class ColliderValueAsset : ValueAsset<Collider>
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

    [Serializable]
    public class ColliderValueFromAsset :
        ValueFromAsset<Collider, ColliderValueAsset>, IColliderValue
    {
    }
}
