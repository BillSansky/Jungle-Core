using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3Int value", fileName = "Vector3IntValue")]
    public class Vector3IntValueAsset : ValueAsset<Vector3Int>
    {
        [SerializeField]
        private Vector3Int value;

        public override Vector3Int Value()
        {
            return value;
        }

        public override void SetValue(Vector3Int value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3Int list value", fileName = "Vector3IntListValue")]
    public class Vector3IntListValueAsset : SerializedValueListAsset<Vector3Int>
    {
    }

    [Serializable]
    public class Vector3IntValueFromAsset :
        ValueFromAsset<Vector3Int, Vector3IntValueAsset>, IVector3IntValue
    {
    }

    [Serializable]
    public class Vector3IntListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector3Int>, Vector3IntListValueAsset>
    {
    }
}
