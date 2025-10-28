using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector2IntValueComponent : ValueComponent<Vector2Int>
    {
        [SerializeField]
        private Vector2Int value;

        public override Vector2Int Value()
        {
            return value;
        }

        public override void SetValue(Vector2Int value)
        {
            this.value = value;
        }
    }

    public class Vector2IntListValueComponent : SerializedValueListComponent<Vector2Int>
    {
    }

    [Serializable]
    public class Vector2IntValueFromComponent :
        ValueFromComponent<Vector2Int, Vector2IntValueComponent>, IVector2IntValue
    {
    }

    [Serializable]
    public class Vector2IntListValueFromComponent :
        ValueFromComponent<IReadOnlyList<Vector2Int>, Vector2IntListValueComponent>
    {
    }
}
