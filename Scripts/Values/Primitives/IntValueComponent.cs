using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class IntValueComponent : ValueComponent<int>, IIntValue
    {
        [SerializeField]
        private int value;

        public override int GetValue()
        {
            return value;
        }
    }
}
