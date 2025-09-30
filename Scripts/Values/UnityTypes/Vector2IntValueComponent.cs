using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector2IntValueComponent : ValueComponent<Vector2Int>
    {
        [SerializeField]
        private Vector2Int value;

        public override Vector2Int GetValue()
        {
            return value;
        }
    }
}
