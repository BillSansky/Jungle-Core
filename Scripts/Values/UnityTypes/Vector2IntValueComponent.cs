using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector2IntValueComponent : ValueComponent<Vector2Int>
    {
        [SerializeField]
        private Vector2Int value;

        public override Vector2Int GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class Vector2IntValueFromComponent :
        ValueFromComponent<Vector2Int, Vector2IntValueComponent>, IVector2IntValue
    {
    }
}
