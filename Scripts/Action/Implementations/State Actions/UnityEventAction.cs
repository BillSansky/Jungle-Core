using Jungle.Actions;
using Jungle.Attributes;

using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    /// <summary>
    /// Invokes a UnityEvent when the state machine enters or exits the action.
    /// </summary>
    [JungleClassInfo("Invokes UnityEvents when actions start, stop, or on one-shot.", "d_UnityEvent Icon")]
    [System.Serializable]
    public class UnityEventAction : IStateAction
    {
        /// <summary>
        /// Handles the OnStateEnter event.
        /// </summary>
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onStop = new();
        [SerializeField] private UnityEvent onOneShot = new();
        
        public void OnStateEnter()
        {
            onStart?.Invoke();
        }
        /// <summary>
        /// Handles the OnStateExit event.
        /// </summary>
        public void OnStateExit()
        {
            onStop?.Invoke();
        }

        
    }
}
