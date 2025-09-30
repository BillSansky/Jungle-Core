using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector2 Value", fileName = "Vector2Value")]
    public class Vector2ValueAsset : ValueAsset<Vector2>, IVector2Value
    {
        [SerializeField]
        private Vector2 value;

        public override Vector2 GetValue()
        {
            return value;
        }
    }
}
