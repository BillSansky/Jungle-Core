using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion value", fileName = "QuaternionValue")]
    public class QuaternionValueAsset : ValueAsset<Quaternion>
    {
        [SerializeField]
        private Quaternion value;

        public override Quaternion Value()
        {
            return value;
        }

        public override void SetValue(Quaternion value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion list value", fileName = "QuaternionListValue")]
    public class QuaternionListValueAsset : SerializedValueListAsset<Quaternion>
    {
    }

    [Serializable]
    public class QuaternionValueFromAsset : ValueFromAsset<Quaternion, QuaternionValueAsset>, IQuaternionValue
    {
    }

    [Serializable]
    public class QuaternionListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Quaternion>, QuaternionListValueAsset>
    {
    }
}
