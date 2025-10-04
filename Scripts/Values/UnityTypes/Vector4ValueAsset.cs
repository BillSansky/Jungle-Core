using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector4 value", fileName = "Vector4Value")]
    public class Vector4ValueAsset : ValueAsset<Vector4>
    {
        [SerializeField]
        private Vector4 value;

        public override Vector4 Value()
        {
            return value;
        }
    }

    [Serializable]
    public class Vector4ValueFromAsset : ValueFromAsset<Vector4, Vector4ValueAsset>, IVector4Value
    {
    }
}
