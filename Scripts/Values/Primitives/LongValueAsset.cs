using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Long Value", fileName = "LongValue")]
    public class LongValueAsset : ValueAsset<long>
    {
        [SerializeField]
        private long value;

        public override long GetValue()
        {
            return value;
        }
    }
}
