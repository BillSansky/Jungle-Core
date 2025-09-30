using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector3ValueComponent : ValueComponent<Vector3>
    {
        [SerializeField]
        private Vector3 value;

        public override Vector3 GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class Vector3ValueFromComponent : ValueFromComponent<Vector3, Vector3ValueComponent>, IVector3Value
    {
    }
}
