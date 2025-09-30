using UnityEngine;

namespace Jungle.Values.Primitives
{
    public class FloatValueComponent : ValueComponent<float>
    {
        [SerializeField]
        private float value;

        public override float GetValue()
        {
            return value;
        }
    }
}
