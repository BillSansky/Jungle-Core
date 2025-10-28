using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector3IntValueComponent : ValueComponent<Vector3Int>
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

    public class Vector3IntListValueComponent : SerializedValueListComponent<Vector3Int>
    {
    }

    [Serializable]
    public class Vector3IntValueFromComponent :
        ValueFromComponent<Vector3Int, Vector3IntValueComponent>, IVector3IntValue
    {
    }

    [Serializable]
    public class Vector3IntListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector3Int>, Vector3IntListValueComponent>
    {
    }
}
