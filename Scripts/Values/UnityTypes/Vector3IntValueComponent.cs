using System;
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

    [Serializable]
    public class Vector3IntValueFromComponent :
        ValueFromComponent<Vector3Int, Vector3IntValueComponent>, IVector3IntValue
    {
    }
}
