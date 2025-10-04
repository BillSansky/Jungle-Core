using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Vector3Int value", fileName = "Vector3IntValue")]
    public class Vector3IntValueAsset : ValueAsset<Vector3Int>
    {
        [SerializeField]
        private Vector3Int value;

        public override Vector3Int Value()
        {
            return value;
        }
    }

    [Serializable]
    public class Vector3IntValueFromAsset :
        ValueFromAsset<Vector3Int, Vector3IntValueAsset>, IVector3IntValue
    {
    }
}
