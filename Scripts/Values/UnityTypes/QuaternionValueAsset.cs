using System;
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
    }

    [Serializable]
    public class QuaternionValueFromAsset : ValueFromAsset<Quaternion, QuaternionValueAsset>, IQuaternionValue
    {
    }
}
