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


        [SerializeReference] private List<IImmediateAction> actions;

        public void OnEnable()
        {
            foreach (var action in actions)
            {
                action?.Start();
            }
        }
        /// <summary>
        /// Unity callback invoked when the component is disabled.
        /// </summary>

        public void OnDisable()
        {
            foreach (var action in actions)
            {
                action?.Stop();
            }
        }
    }
}
