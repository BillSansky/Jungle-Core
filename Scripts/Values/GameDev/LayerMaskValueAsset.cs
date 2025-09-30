using UnityEngine;

namespace Jungle.Values.GameDev
{
    [CreateAssetMenu(menuName = "Jungle/Values/GameDev/LayerMask Value", fileName = "LayerMaskValue")]
    public class LayerMaskValueAsset : ValueAsset<LayerMask>, ILayerMaskValue
    {
        [SerializeField]
        private LayerMask value;

        public override LayerMask GetValue()
        {
            return value;
        }
    }
}
