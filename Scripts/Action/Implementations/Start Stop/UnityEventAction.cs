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
