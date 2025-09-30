using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class IntValueComponent : ValueComponent<int>
    {
        [SerializeField]
        private int value;

        public override int GetValue()
        {
            return value;
        }
    }
}
