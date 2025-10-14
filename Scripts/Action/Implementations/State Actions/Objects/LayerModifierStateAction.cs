using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [System.Serializable]
    public class LayerModifierStateAction : IStateAction
    {
        [SerializeReference] private IGameObjectValue targetObjects = new GameObjectValue();

        [SerializeField] private LayerMask targetLayer;
        [SerializeField] private bool revertOnStop = true;

        private readonly Dictionary<GameObject, int> storedLayers = new();


       public void OnStateEnter()
        {
            if (revertOnStop)
                storedLayers.Clear();

            int layer = GetFirstLayerFromMask(targetLayer);

           
            foreach (var obj in targetObjects.Values)
            {
                if (revertOnStop)
                    storedLayers[obj] = obj.layer;

                obj.layer = layer;
            }
           
        }

        public void OnStateExit()
        {
     
            if (revertOnStop)
            {
                RestoreOriginalLayers();
            }
        }

       

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
