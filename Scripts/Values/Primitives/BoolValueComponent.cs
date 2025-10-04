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
    }

    [Serializable]
    public class BoolValueFromComponent : ValueFromComponent<bool, BoolValueComponent>, IBoolValue
    {
    }
}
