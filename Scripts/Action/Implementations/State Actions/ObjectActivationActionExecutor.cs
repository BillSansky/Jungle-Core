using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Toggles the active state of multiple GameObjects according to the configured timing.
    /// </summary>
    public class ObjectActivationActionExecutor : MonoBehaviour
    {
        /// <summary>
        /// Handles the OnEnable event.
        /// </summary>
        [SerializeReference] private List<IStateAction> actions;

        public void OnEnable()
        {
            foreach (var action in actions)
            {
                action.OnStateEnter();
            }
        }
        /// <summary>
        /// Handles the OnDisable event.
        /// </summary>
        public void OnDisable()
        {
            foreach (var action in actions)
            {
               action.OnStateExit();
            }
        }
    }
}
