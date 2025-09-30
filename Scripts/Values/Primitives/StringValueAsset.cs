using System;
using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/String Value", fileName = "StringValue")]
    public class StringValueAsset : ValueAsset<string>
    {
        [SerializeField]
        private string value;

        public override string GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class StringValueFromAsset : ValueFromAsset<string, StringValueAsset>, IStringValue
    {
    }
}
