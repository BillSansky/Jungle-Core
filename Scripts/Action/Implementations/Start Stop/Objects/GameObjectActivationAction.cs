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
    public class GameObjectActivationAction : ProcessAction
    {
        private enum ActivationState
        {
            Enable,
            Disable,
            Toggle
        }

        [SerializeReference] private IGameObjectValue targetObjects = new GameObjectLocalArrayValue();
        [SerializeField] private ActivationState actionStart = ActivationState.Enable;
        [SerializeField] private ActivationState actionStop = ActivationState.Toggle;

        private readonly Dictionary<GameObject, bool> originalStates = new();

        public override bool IsTimed => false;
        public override float Duration => 0f;

        public void StartAction()
        {
            Start();
        }

        public void StopAction()
        {
            Stop();
        }

        protected override void OnStart()
        {
            StoreOriginalStates();
            SetObjectStates();
        }

        protected override void OnStop()
        {
            RestoreOriginalStates();
        }

      

        private void StoreOriginalStates()
        {
            originalStates.Clear();
            
            foreach (var obj in targetObjects.Values)
            {
                if (obj == null || originalStates.ContainsKey(obj))
                {
                    continue;
                }

                originalStates[obj] = obj.activeSelf;
            }
        }

        private void SetObjectStates()
        {
            foreach (var obj in targetObjects.Values)
            {
                ApplyAction(obj, actionStart, false);
            }
        }

        private void RestoreOriginalStates()
        {
            foreach (var obj in targetObjects.Values)
            {
                ApplyAction(obj, actionStop, true);
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
