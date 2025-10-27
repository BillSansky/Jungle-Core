using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector4ValueComponent : ValueComponent<Vector4>
    {
        [SerializeField]
        private Vector4 value;

        public override Vector4 Value()
        {
            return value;
        }

        public override void SetValue(Vector4 value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class Vector4ValueFromComponent : ValueFromComponent<Vector4, Vector4ValueComponent>, IVector4Value
    {
    }
}
