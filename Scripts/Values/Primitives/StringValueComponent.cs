using System;
using System.Collections.Generic;
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

    public class StringListValueComponent : SerializedValueListComponent<string>
    {
    }

    [Serializable]
    public class StringValueFromComponent : ValueFromComponent<string, StringValueComponent>, IStringValue
    {
    }

    [Serializable]
    public class StringListValueFromComponent : ValueFromComponent<IReadOnlyList<string>, StringListValueComponent>
    {
    }
}
