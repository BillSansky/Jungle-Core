using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3 value", fileName = "Vector3Value")]
    public class Vector3ValueAsset : ValueAsset<Vector3>
    {
        [SerializeField]
        private Vector3 value;

        public override Vector3 Value()
        {
            return value;
        }

        public override void SetValue(Vector3 value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3 list value", fileName = "Vector3ListValue")]
    public class Vector3ListValueAsset : SerializedValueListAsset<Vector3>
    {
    }

    [Serializable]
    public class Vector3ValueFromAsset : ValueFromAsset<Vector3, Vector3ValueAsset>, IVector3Value
    {
    }

    [Serializable]
    public class Vector3ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector3>, Vector3ListValueAsset>
    {
    }
}
