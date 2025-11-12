using System;
using Jungle.Attributes;

using Jungle.Values.GameDev;
using UnityEngine;

namespace Jungle.Actions
{
    [JungleClassInfo(
        "GameObject Activation Action",
        "Controls the active state of GameObjects when the action executes.",
        "d_GameObject Icon",
        "Actions/GameObject")]
    /// <summary>
    /// Implements the game object activation state action action.
    /// </summary>
    [Serializable]
    public class GameObjectActivationStateAction : IImmediateAction
    {
        private enum ActivationState
        {
            Enable,
            Disable,
            Toggle
        }

        [SerializeReference] private IGameObjectValue targetObjects = new GameObjectLocalArrayValue();
        [SerializeField] private ActivationState beginAction = ActivationState.Enable;

        public void StartProcess(Action callback = null)
        {
            SetObjectStates();
            callback?.Invoke();
        }

        private void SetObjectStates()
        {
            foreach (var obj in targetObjects.Values)
            {
                ApplyAction(obj, beginAction);
            }
        }

        private void ApplyAction(GameObject obj, ActivationState state)
        {
            if (obj == null)
            {
                return;
            }

            switch (state)
            {
                case ActivationState.Enable:
                    obj.SetActive(true);
                    break;
                case ActivationState.Disable:
                    obj.SetActive(false);
                    break;
                case ActivationState.Toggle:
                    obj.SetActive(!obj.activeSelf);
                    break;
            }
        }

     
    }
}
