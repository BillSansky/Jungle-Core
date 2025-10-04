using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class QuaternionValueComponent : ValueComponent<Quaternion>
    {
        [SerializeField]
        private Quaternion value;

        public override Quaternion Value()
        {
            return value;
        }
    }

    [Serializable]
    public class QuaternionValueFromComponent : ValueFromComponent<Quaternion, QuaternionValueComponent>,
        IQuaternionValue
    {
    }
}
