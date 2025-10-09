using Jungle.Actions;
using Jungle.Attributes;

using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    [JungleClassInfo("Invokes UnityEvents when actions start, stop, or on one-shot.", "d_UnityEvent Icon")]
    [System.Serializable]
    public class UnityEventAction : ProcessAction
    {
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onStop = new();
        [SerializeField] private UnityEvent onOneShot = new();
        
        public override bool IsTimed => false;
        public override float Duration => 0f;

        
        protected override void BeginImpl()
        {
            onStart?.Invoke();
        }

        protected override void CompleteImpl()
        {
            onStop?.Invoke();
        }

        
    }
}
