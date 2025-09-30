using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class GradientValueComponent : ValueComponent<Gradient>, IGradientValue
    {
        [SerializeField]
        private Gradient value = new Gradient();

        public override Gradient GetValue()
        {
            return value;
        }
    }
}
