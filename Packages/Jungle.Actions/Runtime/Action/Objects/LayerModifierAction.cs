using System;
using System.Collections.Generic;
using Jungle.Actions;
using Jungle.Attributes;
using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Changes the layer of target GameObjects while the state is active.
    /// </summary>
    [Serializable]
    [JungleClassInfo("Layer Modifier Action", "Changes the layer of target GameObjects while the state is active.", null, "Actions/State")]
    public class LayerModifierAction : IImmediateAction
    {
        [SerializeReference][JungleClassSelection] private IGameObjectValue targetObjects = new GameObjectValue();

        [SerializeField] private LayerMask targetLayer;
       
        public void StartProcess(Action callback = null)
        {

            int layer = GetFirstLayerFromMask(targetLayer);

            foreach (var obj in targetObjects.Values)
            {
                obj.layer = layer;
            }

           
            callback?.Invoke();
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
