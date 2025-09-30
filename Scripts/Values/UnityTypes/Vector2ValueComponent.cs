using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector2ValueComponent : ValueComponent<Vector2>
    {
        [SerializeField]
        private Vector2 value;

        public override Vector2 GetValue()
        {
            return value;
        }
    }
}
