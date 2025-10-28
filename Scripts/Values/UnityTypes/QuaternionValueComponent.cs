using System;
using System.Collections.Generic;
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

        public override void SetValue(Quaternion value)
        {
            this.value = value;
        }
    }

    public class QuaternionListValueComponent : SerializedValueListComponent<Quaternion>
    {
    }

    [Serializable]
    public class QuaternionValueFromComponent : ValueFromComponent<Quaternion, QuaternionValueComponent>,
        IQuaternionValue
    {
    }

    [Serializable]
    public class QuaternionListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Quaternion>, QuaternionListValueComponent>
    {
    }
}
