using UnityEngine;

namespace Jungle.Values.UnityTypes
{
    [CreateAssetMenu(menuName = "Jungle/Values/Unity/Gradient Value", fileName = "GradientValue")]
    public class GradientValueAsset : ValueAsset<Gradient>, IGradientValue
    {
        [SerializeField]
        private Gradient value = new Gradient();

        public override Gradient GetValue()
        {
            return value;
        }
    }
}
