using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class TransformValueComponent : ValueComponent<Transform>
    {
        [SerializeField]
        private Transform value;

        public override Transform Value()
        {
            return value;
        }

        public override void SetValue(Transform value)
        {
            this.value = value;
        }
    }

    public class TransformListValueComponent : SerializedValueListComponent<Transform>
    {
    }

    [Serializable]
    public class TransformValueFromComponent :
        ValueFromComponent<Transform, TransformValueComponent>, ITransformValue
    {
    }

    [Serializable]
    public class TransformListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Transform>, TransformListValueComponent>
    {
    }
}
