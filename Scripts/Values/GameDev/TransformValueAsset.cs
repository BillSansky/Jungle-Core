using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/Transform Value", fileName = "TransformValue")]
    public class TransformValueAsset : ValueAsset<Transform>
    {
        [SerializeField]
        private Transform value;

        public override Transform GetValue()
        {
            return value;
        }
    }
}
