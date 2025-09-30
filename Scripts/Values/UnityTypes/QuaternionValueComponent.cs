using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class QuaternionValueComponent : ValueComponent<Quaternion>, IQuaternionValue
    {
        [SerializeField]
        private Quaternion value;

        public override Quaternion GetValue()
        {
            return value;
        }
    }
}
