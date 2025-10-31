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
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onStop = new();
        [SerializeField] private UnityEvent onOneShot = new();

        /// <summary>
        /// Invokes the configured start event when the action begins.
        /// </summary>
        public void OnStateEnter()
        {
            onStart?.Invoke();
        }
        /// <summary>
        /// Invokes the configured stop event when the action ends.
        /// </summary>
        public void OnStateExit()
        {
            onStop?.Invoke();
        }

        
    }
}
