using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/String value", fileName = "StringValue")]
    public class StringValueAsset : ValueAsset<string>
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

    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/String list value", fileName = "StringListValue")]
    public class StringListValueAsset : SerializedValueListAsset<string>
    {
    }

    [Serializable]
    public class StringValueFromAsset : ValueFromAsset<string, StringValueAsset>, IStringValue
    {
    }

    [Serializable]
    public class StringListValueFromAsset : ValueFromAsset<IReadOnlyList<string>, StringListValueAsset>
    {
    }
}
