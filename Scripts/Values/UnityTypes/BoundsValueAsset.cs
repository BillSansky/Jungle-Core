using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Bounds value", fileName = "BoundsValue")]
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
    public class BoundsListValueAsset : SerializedValueListAsset<Bounds>
    {
    }

    [Serializable]
    public class BoundsValueFromAsset : ValueFromAsset<Bounds, BoundsValueAsset>, IBoundsValue
    {
    }

    [Serializable]
    public class BoundsListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Bounds>, BoundsListValueAsset>
    {
    }
}
