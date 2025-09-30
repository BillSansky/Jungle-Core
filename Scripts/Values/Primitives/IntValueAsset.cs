using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Int Value", fileName = "IntValue")]
    public class IntValueAsset : ValueAsset<int>, IIntValue
    {
        [SerializeField]
        private int value;

        public override int GetValue()
        {
            return value;
        }
    }
}
