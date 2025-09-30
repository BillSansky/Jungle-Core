using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class Vector3IntValueComponent : ValueComponent<Vector3Int>
    {
        [SerializeField]
        private Vector3Int value;

        public override Vector3Int GetValue()
        {
            return value;
        }
    }
}
