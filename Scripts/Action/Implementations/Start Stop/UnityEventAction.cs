using Jungle.Actions;
using Jungle.Attributes;

using UnityEngine;
using UnityEngine.Events;

namespace Jungle.Actions
{
    [JungleClassInfo("Invokes UnityEvents when actions start, stop, or on one-shot.", "d_UnityEvent Icon")]
    [System.Serializable]
    public class UnityEventAction : BeginCompleteAction
    {
        [SerializeField] private UnityEvent onStart = new();
        [SerializeField] private UnityEvent onStop = new();
        [SerializeField] private UnityEvent onOneShot = new();
        
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
            onStart?.Invoke();
        }

        protected override void OnStop()
        {
            onStop?.Invoke();
        }

        
    }
}
