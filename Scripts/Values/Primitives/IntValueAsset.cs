using System;
using System.Collections.Generic;
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

        public override void SetValue(int value)
        {
            this.value = value;
        }
    }

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int list value", fileName = "IntListValue")]
    public class IntListValueAsset : SerializedValueListAsset<int>
    {
    }

    [Serializable]
    public class IntValueFromAsset : ValueFromAsset<int, IntValueAsset>, IIntValue
    {
    }

    [Serializable]
    public class IntListValueFromAsset : ValueFromAsset<IReadOnlyList<int>, IntListValueAsset>
    {
    }
}
