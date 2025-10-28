using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2Int value", fileName = "Vector2IntValue")]
    public class Vector2IntValueAsset : ValueAsset<Vector2Int>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2Int list value", fileName = "Vector2IntListValue")]
    public class Vector2IntListValueAsset : SerializedValueListAsset<Vector2Int>
    {
    }

    [Serializable]
    public class Vector2IntValueFromAsset :
        ValueFromAsset<Vector2Int, Vector2IntValueAsset>, IVector2IntValue
    {
    }

    [Serializable]
    public class Vector2IntListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector2Int>, Vector2IntListValueAsset>
    {
    }
}
