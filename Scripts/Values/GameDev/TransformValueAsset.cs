using System;
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
    }

    [Serializable]
    public class TransformValueFromAsset :
        ValueFromAsset<Transform, TransformValueAsset>, ITransformValue
    {
    }
}
