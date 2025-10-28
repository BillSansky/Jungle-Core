using System;
using System.Collections.Generic;
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

        public override void SetValue(Vector4 value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector4 list value", fileName = "Vector4ListValue")]
    public class Vector4ListValueAsset : SerializedValueListAsset<Vector4>
    {
    }

    [Serializable]
    public class Vector4ValueFromAsset : ValueFromAsset<Vector4, Vector4ValueAsset>, IVector4Value
    {
    }

    [Serializable]
    public class Vector4ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector4>, Vector4ListValueAsset>
    {
    }
}
