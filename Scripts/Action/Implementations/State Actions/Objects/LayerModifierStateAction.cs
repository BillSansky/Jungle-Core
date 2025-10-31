using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Overrides the layer of selected GameObjects while the state is active.
    /// </summary>
    [System.Serializable]
    public class LayerModifierStateAction : IStateAction
    {
        [SerializeReference] private IGameObjectValue targetObjects = new GameObjectValue();

        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private bool revertOnStop = true;

        private readonly Dictionary<GameObject, int> storedLayers = new();

        /// <summary>
        /// Saves current layer assignments and applies the override layer when the state starts.
        /// </summary>
        public void OnStateEnter()
        {
            if (revertOnStop)
                storedLayers.Clear();

            int layer = GetFirstLayerFromMask(targetLayer);

           
            foreach (var obj in targetObjects.Gs)
            {
                if (revertOnStop)
                    storedLayers[obj] = obj.layer;

                obj.layer = layer;
            }
           
        }
        /// <summary>
        /// Restores the stored layers when the state completes if revert is enabled.
        /// </summary>
        public void OnStateExit()
        {
     
            if (revertOnStop)
            {
                RestoreOriginalLayers();
            }
        }
        /// <summary>
        /// Restores previously captured layer assignments when the action ends.
        /// </summary>
        private void RestoreOriginalLayers()
        {
            foreach (var pair in storedLayers)
            {
                if (pair.Key != null)
                {
                    pair.Key.layer = pair.Value;
                }
            }

            storedLayers.Clear();
        }
        /// <summary>
        /// Extracts the first layer index contained within the provided mask.
        /// </summary>
        private int GetFirstLayerFromMask(LayerMask layerMask)
        {
            int bitmask = layerMask.value;
            for (int i = 0; i < 32; i++)
            {
                if ((bitmask & (1 << i)) != 0)
                {
                    return i;
                }
            }

            return 0; // Default layer if no layer is found in the mask
        }

#if UNITY_EDITOR


        private int previousLayerValue;
        /// <summary>
        /// Keeps the editor field constrained to a single selected layer.
        /// </summary>
        private void EnsureSingleLayerSelection()
        {
            if (targetLayer.value == 0) return; // No layer selected

            if (targetLayer.value != previousLayerValue)
            {
                int diff = targetLayer.value ^ previousLayerValue;
                int latestLayer = diff & targetLayer.value;
                targetLayer = latestLayer;
                previousLayerValue = targetLayer.value;
            }
        }
#endif
    
    }
}
