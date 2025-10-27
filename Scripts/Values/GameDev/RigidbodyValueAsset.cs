using System;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Rigidbody value", fileName = "RigidbodyValue")]
    public class RigidbodyValueAsset : ValueAsset<Rigidbody>
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

    [Serializable]
    public class RigidbodyValueFromAsset :
        ValueFromAsset<Rigidbody, RigidbodyValueAsset>, IRigidbodyValue
    {
    }
}
