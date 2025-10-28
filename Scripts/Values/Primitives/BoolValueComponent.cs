using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class BoolValueComponent : ValueComponent<bool>
    {
        [SerializeField]
        private bool value;

        public override bool Value()
        {
            return value;
        }

        public override void SetValue(bool value)
        {
            this.value = value;
        }
    }

    public class BoolListValueComponent : SerializedValueListComponent<bool>
    {
    }

    [Serializable]
    public class BoolValueFromComponent : ValueFromComponent<bool, BoolValueComponent>, IBoolValue
    {
    }

    [Serializable]
    public class BoolListValueFromComponent : ValueFromComponent<IReadOnlyList<bool>, BoolListValueComponent>
    {
    }
}
