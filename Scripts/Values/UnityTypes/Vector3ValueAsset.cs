using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3 Value", fileName = "Vector3Value")]
    public class Vector3ValueAsset : ValueAsset<Vector3>
    {
        [SerializeField]
        private Vector3 value;

        public override Vector3 GetValue()
        {
            return value;
        }
    }
}
