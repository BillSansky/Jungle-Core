using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class BoolValueComponent : ValueComponent<bool>
    {
        [SerializeField]
        private bool value;

        public override bool GetValue()
        {
            return value;
        }
    }
}
