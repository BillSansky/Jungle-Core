using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/String Value", fileName = "StringValue")]
    public class StringValueAsset : ValueAsset<string>, IStringValue
    {
        [SerializeField]
        private string value;

        public override string GetValue()
        {
            return value;
        }
    }
}
