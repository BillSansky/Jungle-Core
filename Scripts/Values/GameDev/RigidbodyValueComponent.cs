using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class RigidbodyValueComponent : ValueComponent<Rigidbody>
    {
        [SerializeField]
        private Rigidbody value;

        public override Rigidbody Value()
        {
            return value;
        }

        public override void SetValue(Rigidbody value)
        {
            this.value = value;
        }
    }

    public class RigidbodyListValueComponent : SerializedValueListComponent<Rigidbody>
    {
    }

    [Serializable]
    public class RigidbodyValueFromComponent :
        ValueFromComponent<Rigidbody, RigidbodyValueComponent>, IRigidbodyValue
    {
    }

    [Serializable]
    public class RigidbodyListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Rigidbody>, RigidbodyListValueComponent>
    {
    }
}
