using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 value", fileName = "Vector2Value")]
    public class Vector2ValueAsset : ValueAsset<Vector2>
    {
        [SerializeField]
        private Vector2 value;

        public override Vector2 Value()
        {
            return value;
        }
    }

    [Serializable]
    public class Vector2ValueFromAsset : ValueFromAsset<Vector2, Vector2ValueAsset>, IVector2Value
    {
    }
}
