using System;
using System.Collections.Generic;
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

        public override void SetValue(Vector2 value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 list value", fileName = "Vector2ListValue")]
    public class Vector2ListValueAsset : SerializedValueListAsset<Vector2>
    {
    }

    [Serializable]
    public class Vector2ValueFromAsset : ValueFromAsset<Vector2, Vector2ValueAsset>, IVector2Value
    {
    }

    [Serializable]
    public class Vector2ListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Vector2>, Vector2ListValueAsset>
    {
    }
}
