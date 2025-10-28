using System;
using System.Collections.Generic;
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Rigidbody list value", fileName = "RigidbodyListValue")]
    public class RigidbodyListValueAsset : SerializedValueListAsset<Rigidbody>
    {
    }

    [Serializable]
    public class RigidbodyValueFromAsset :
        ValueFromAsset<Rigidbody, RigidbodyValueAsset>, IRigidbodyValue
    {
    }

    [Serializable]
    public class RigidbodyListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Rigidbody>, RigidbodyListValueAsset>
    {
    }
}
