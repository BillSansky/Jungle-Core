using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Toggles the active state of multiple GameObjects according to the configured timing.
    /// </summary>
    public class ObjectActivationActionExecutor : MonoBehaviour
    {
        [SerializeReference] private List<IStateAction> actions;

        /// <summary>
        /// Notifies each state action that the host object has been enabled.
        /// </summary>
        public void OnEnable()
        {
            foreach (var action in actions)
            {
                action.OnStateEnter();
            }
        }
        /// <summary>
        /// Signals to the state actions that the object is disabling so they can clean up.
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
