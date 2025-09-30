using UnityEngine;

namespace Jungle.Values.GameDev
{
    public class TransformValueComponent : ValueComponent<Transform>, ITransformValue
    {
        [SerializeField]
        private Transform value;

        public override Transform GetValue()
        {
            return value;
        }
    }
}
