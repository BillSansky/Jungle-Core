using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform value", fileName = "TransformLocalValue")]
    public class TransformValueAsset : ValueAsset<Transform>
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

    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform list value", fileName = "TransformListValue")]
    public class TransformListValueAsset : SerializedValueListAsset<Transform>
    {
    }

    [Serializable]
    public class TransformValueFromAsset :
        ValueFromAsset<Transform, TransformValueAsset>, ITransformValue
    {
    }

    [Serializable]
    public class TransformListValueFromAsset :
        ValueFromAsset<IReadOnlyList<Transform>, TransformListValueAsset>
    {
    }
}
