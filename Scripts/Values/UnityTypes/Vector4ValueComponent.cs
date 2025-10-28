using System;
using System.Collections.Generic;
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

    public class Vector4ListValueComponent : SerializedValueListComponent<Vector4>
    {
    }

    [Serializable]
    public class Vector4ValueFromComponent : ValueFromComponent<Vector4, Vector4ValueComponent>, IVector4Value
    {
    }

    [Serializable]
    public class Vector4ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector4>, Vector4ListValueComponent>
    {
    }
}
