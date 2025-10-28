using System;
using System.Collections.Generic;
using Jungle.Attributes;

using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "Controls the active state of GameObjects. Configure start and stop actions to enable, disable or toggle targets.",
        "d_GameObject Icon")]
    [Serializable]
    public class GameObjectActivationStateAction : IStateAction
    {
        private enum ActivationState
        {
            Enable,
            Disable,
            Toggle
        }

        [SerializeReference] private IGameObjectValue targetObjects = new GameObjectLocalArrayValue();
        [SerializeField] private ActivationState beginAction = ActivationState.Enable;
        [SerializeField] private ActivationState endAction = ActivationState.Toggle;

        private readonly Dictionary<GameObject, bool> originalStates = new();
        

        public void OnStateEnter()
        {
            StoreOriginalStates();
            SetObjectStates();
        }

        public void OnStateExit()
        {
            RestoreOriginalStates();
        }

      

        private void StoreOriginalStates()
        {
            originalStates.Clear();
            
            foreach (var obj in targetObjects.Gs)
            {
                originalStates.Add(obj,obj.activeSelf);
            }
        }

        private void SetObjectStates()
        {
            foreach (var obj in targetObjects.Gs)
            {
                ApplyAction(obj, beginAction, false);
            }
        }

        private void RestoreOriginalStates()
        {
            foreach (var obj in targetObjects.Gs)
            {
                ApplyAction(obj, endAction, true);
            }

            originalStates.Clear();
        }

        private void ApplyAction(GameObject obj, ActivationState state, bool revertToOriginal)
        {
            if (obj == null)
            {
                return;
            }

            var hasOriginalState = originalStates.TryGetValue(obj, out bool originalState);
            var currentState = obj.activeSelf;

            switch (state)
            {
                case ActivationState.Enable:
                    obj.SetActive(true);
                    break;
                case ActivationState.Disable:
                    obj.SetActive(false);
                    break;
                case ActivationState.Toggle:
                    if (revertToOriginal && hasOriginalState)
                    {
                        obj.SetActive(originalState);
                    }
                    else if (revertToOriginal)
                    {
                        obj.SetActive(currentState);
                    }
                    else if (hasOriginalState)
                    {
                        obj.SetActive(!originalState);
                    }
                    else
                    {
                        obj.SetActive(!currentState);
                    }
                    break;
            }
        }

     
    }
}
