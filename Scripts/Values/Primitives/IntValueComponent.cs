using System;
using System.Collections.Generic;
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

    public class IntListValueComponent : SerializedValueListComponent<int>
    {
    }

    [Serializable]
    public class IntValueFromComponent : ValueFromComponent<int, IntValueComponent>, IIntValue
    {
    }

    [Serializable]
    public class IntListValueFromComponent : ValueFromComponent<IReadOnlyList<int>, IntListValueComponent>
    {
    }
}
