using System;
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
    }

    [Serializable]
    public class Vector2IntValueFromAsset :
        ValueFromAsset<Vector2Int, Vector2IntValueAsset>, IVector2IntValue
    {
    }
}
