using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Bounds Value", fileName = "BoundsValue")]
    public class BoundsValueAsset : ValueAsset<Bounds>
    {
        [SerializeField]
        private Bounds value = new Bounds(Vector3.zero, Vector3.one);

        public override Bounds GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class BoundsValueFromAsset : ValueFromAsset<Bounds, BoundsValueAsset>, IBoundsValue
    {
    }
}
