using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector2ValueComponent : ValueComponent<Vector2>
    {
        [SerializeField]
        private Vector2 value;

        public override Vector2 Value()
        {
            return value;
        }

        public override void SetValue(Vector2 value)
        {
            this.value = value;
        }
    }

    public class Vector2ListValueComponent : SerializedValueListComponent<Vector2>
    {
    }

    [Serializable]
    public class Vector2ValueFromComponent : ValueFromComponent<Vector2, Vector2ValueComponent>, IVector2Value
    {
    }

    [Serializable]
    public class Vector2ListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector2>, Vector2ListValueComponent>
    {
    }
}
