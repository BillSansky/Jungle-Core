using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int value", fileName = "IntValue")]
    public class IntValueAsset : ValueAsset<int>
    {
        [SerializeField]
        private int value;

        public override int Value()
        {
            return value;
        }
    }

    [Serializable]
    public class IntValueFromAsset : ValueFromAsset<int, IntValueAsset>, IIntValue
    {
    }
}
