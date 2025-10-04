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
    }

    [Serializable]
    public class StringValueFromComponent : ValueFromComponent<string, StringValueComponent>, IStringValue
    {
    }
}
