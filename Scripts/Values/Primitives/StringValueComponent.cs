using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class StringValueComponent : ValueComponent<string>
    {
        [SerializeField]
        private string value;

        public override string Value()
        {
            return value;
        }

        public override void SetValue(string value)
        {
            this.value = value;
        }
    }

    [Serializable]
    public class StringValueFromComponent : ValueFromComponent<string, StringValueComponent>, IStringValue
    {
    }
}
