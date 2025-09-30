using System;
using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Gradient Value", fileName = "GradientValue")]
    public class GradientValueAsset : ValueAsset<Gradient>
    {
        [SerializeField]
        private Gradient value = new Gradient();

        public override Gradient GetValue()
        {
            return value;
        }
    }

    [Serializable]
    public class GradientValueFromAsset : ValueFromAsset<Gradient, GradientValueAsset>, IGradientValue
    {
    }
}
