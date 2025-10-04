using System;
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
    }

    [Serializable]
    public class RigidbodyValueFromComponent :
        ValueFromComponent<Rigidbody, RigidbodyValueComponent>, IRigidbodyValue
    {
    }
}
