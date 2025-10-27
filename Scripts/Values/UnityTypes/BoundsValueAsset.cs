using System;
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

    [Serializable]
    public class BoundsValueFromAsset : ValueFromAsset<Bounds, BoundsValueAsset>, IBoundsValue
    {
    }
}
