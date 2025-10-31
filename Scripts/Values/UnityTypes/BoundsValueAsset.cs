using System;
using System.Collections.Generic;
using Jungle.Attributes;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Bounds value", fileName = "BoundsValue")]
    [JungleClassInfo("Bounds Value Asset", "ScriptableObject storing a Bounds value.", null, "Values/Unity Types")]
    public class BoundsValueAsset : ValueAsset<Bounds>
    {
        [SerializeField]
        private Bounds value = new Bounds(Vector3.zero, Vector3.one);

        public override Bounds Value()
        {
            return value;
        }

        public override void SetValue(Bounds value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Bounds list value", fileName = "BoundsListValue")]
    [JungleClassInfo("Bounds List Asset", "ScriptableObject storing a list of Bounds values.", null, "Values/Unity Types")]
    public class BoundsListValueAsset : SerializedValueListAsset<Bounds>
    {
    }

    [Serializable]
    [JungleClassInfo("Bounds Value From Asset", "Reads a Bounds value from a BoundsValueAsset.", null, "Values/Unity Types")]
    public class BoundsValueFromAsset : ValueFromAsset<Bounds, BoundsValueAsset>, IBoundsValue
    {
    }

    [Serializable]
    [JungleClassInfo("Bounds List From Asset", "Reads Bounds values from a BoundsListValueAsset.", null, "Values/Unity Types")]
    public class BoundsListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Bounds>, BoundsListValueAsset>
    {
    }
}
