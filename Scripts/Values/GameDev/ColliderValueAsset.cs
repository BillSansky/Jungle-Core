using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collider value", fileName = "ColliderLocalValue")]
    [JungleClassInfo("Collider Value Asset", "ScriptableObject storing a collider component.", null, "Values/Game Dev")]
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Collider list value", fileName = "ColliderListValue")]
    [JungleClassInfo("Collider List Asset", "ScriptableObject storing a list of colliders.", null, "Values/Game Dev")]
    public class ColliderListValueAsset : SerializedValueListAsset<Collider>
    {
    }

    [Serializable]
    [JungleClassInfo("Collider Value From Asset", "Reads a collider component from a ColliderValueAsset.", null, "Values/Game Dev")]
    public class ColliderValueFromAsset :
        ValueFromAsset<Collider, ColliderValueAsset>, IColliderValue
    {
    }

    [Serializable]
    [JungleClassInfo("Collider List From Asset", "Reads colliders from a ColliderListValueAsset.", null, "Values/Game Dev")]
    public class ColliderListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Collider>, ColliderListValueAsset>
    {
    }
}
