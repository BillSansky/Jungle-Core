using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class LongValueComponent : ValueComponent<long>, ILongValue
    {
        [SerializeField]
        private long value;

        public override long GetValue()
        {
            return value;
        }
    }
}
