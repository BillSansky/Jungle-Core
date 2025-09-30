using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class StringValueComponent : ValueComponent<string>, IStringValue
    {
        [SerializeField]
        private string value;

        public override string GetValue()
        {
            return value;
        }
    }
}
