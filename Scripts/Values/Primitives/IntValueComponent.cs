using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class IntValueComponent : ValueComponent<int>
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

    [Serializable]
    public class IntValueFromComponent : ValueFromComponent<int, IntValueComponent>, IIntValue
    {
    }
}
