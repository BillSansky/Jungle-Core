using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector4ValueComponent : ValueComponent<Vector4>, IVector4Value
    {
        [SerializeField]
        private Vector4 value;

        public override Vector4 GetValue()
        {
            return value;
        }
    }
}
