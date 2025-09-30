using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2Int Value", fileName = "Vector2IntValue")]
    public class Vector2IntValueAsset : ValueAsset<Vector2Int>, IVector2IntValue
    {
        [SerializeField]
        private Vector2Int value;

        public override Vector2Int GetValue()
        {
            return value;
        }
    }
}
