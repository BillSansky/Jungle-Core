using UnityEngine;

namespace Jungle.Values.Primitives
{
    [CreateAssetMenu(menuName = "Jungle/Values/Primitives/Double Value", fileName = "DoubleValue")]
    public class DoubleValueAsset : ValueAsset<double>, IDoubleValue
    {
        [SerializeField]
        private double value;

        public override double GetValue()
        {
            return value;
        }
    }
}
