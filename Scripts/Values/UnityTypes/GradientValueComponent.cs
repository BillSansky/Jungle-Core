using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    public class GradientValueComponent : ValueComponent<Gradient>
    {
        [SerializeField]
        private Gradient value = new Gradient();

        public override Gradient GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class GradientValueFromComponent : ValueFromComponent<Gradient, GradientValueComponent>, IGradientValue
    {
    }
}
