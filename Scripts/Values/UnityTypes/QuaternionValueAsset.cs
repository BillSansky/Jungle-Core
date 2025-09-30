using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion Value", fileName = "QuaternionValue")]
    public class QuaternionValueAsset : ValueAsset<Quaternion>
    {
        [SerializeField]
        private Quaternion value;

        public override Quaternion GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class QuaternionValueFromAsset : ValueFromAsset<Quaternion, QuaternionValueAsset>, IQuaternionValue
    {
    }
}
