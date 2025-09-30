using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Quaternion Value", fileName = "QuaternionValue")]
    public class QuaternionValueAsset : ValueAsset<Quaternion>, IQuaternionValue
    {
        [SerializeField]
        private Quaternion value;

        public override Quaternion GetValue()
        {
            return value;
        }
    }
}
