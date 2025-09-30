using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3Int Value", fileName = "Vector3IntValue")]
    public class Vector3IntValueAsset : ValueAsset<Vector3Int>, IVector3IntValue
    {
        [SerializeField]
        private Vector3Int value;

        public override Vector3Int GetValue()
        {
            return value;
        }
    }
}
