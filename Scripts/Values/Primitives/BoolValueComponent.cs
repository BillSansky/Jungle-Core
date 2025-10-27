using System;
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

    [Serializable]
    public class BoolValueFromComponent : ValueFromComponent<bool, BoolValueComponent>, IBoolValue
    {
    }
}
