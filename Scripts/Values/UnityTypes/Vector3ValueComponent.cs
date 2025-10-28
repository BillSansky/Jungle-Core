using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector3ValueComponent : ValueComponent<Vector3>
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

    public class Vector3ListValueComponent : SerializedValueListComponent<Vector3>
    {
    }

    [Serializable]
    public class Vector3ValueFromComponent : ValueFromComponent<Vector3, Vector3ValueComponent>, IVector3Value
    {
    }

    [Serializable]
    public class Vector3ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector3>, Vector3ListValueComponent>
    {
    }
}
