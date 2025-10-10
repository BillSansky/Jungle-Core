using Jungle.Actions;
using Jungle.Attributes;

using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    [JungleClassInfo("Invokes UnityEvents when actions start, stop, or on one-shot.", "d_UnityEvent Icon")]
    [System.Serializable]
    public class UnityEventAction : IBeginEndAction
    {
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onStop = new();
        [SerializeField] private UnityEvent onOneShot = new();
        
        public void Begin()
        {
            onStart?.Invoke();
        }

        public void End()
        {
            onStop?.Invoke();
        }

        
    }
}
