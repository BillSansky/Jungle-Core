using System;
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
    }

    [Serializable]
    public class StringValueFromAsset : ValueFromAsset<string, StringValueAsset>, IStringValue
    {
    }
}
