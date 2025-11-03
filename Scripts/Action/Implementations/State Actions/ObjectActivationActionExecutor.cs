using System.Collections.Generic;
using UnityEngine;

namespace Jungle.Actions
{
    /// <summary>
    /// Implements the object activation action executor action.
    /// </summary>
    public class ObjectActivationActionExecutor : MonoBehaviour
    {
        /// <summary>
        /// Unity callback invoked when the component is enabled.
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
        /// Unity callback invoked when the component is disabled.
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
